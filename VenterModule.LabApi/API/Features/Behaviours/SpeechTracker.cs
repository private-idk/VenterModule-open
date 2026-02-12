using System;
using LabApi.Features.Wrappers;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
using PlayerRoles.Voice;
using UnityEngine;
using VoiceChat.Codec;
using VoiceChat.Networking;

namespace Talky
{
    public class SpeechTracker : MonoBehaviour
    {
        public Player player;

        public ReferenceHub hub => player.ReferenceHub;
        public Player Proxy { get; set; }
        
        public PlaybackBuffer buffer;
        private EmotionPresetType _defaultPreset = EmotionPresetType.Neutral;
        
        private EmotionPresetType _overridePreset = EmotionPresetType.Neutral;
        private long _overrideEndTime = 0;
        
        
        public OpusDecoder OpusDecoder
        {
            get
            {
                return player.VoiceModule.Decoder;
            }
        }

        public EmotionPresetType DefaultPreset => EmotionPresetType.Neutral;
        
        public long LastPacketTime { get; set; }
        
        public int LastLevel { get; private set; }



        // Use this for initialization
        void Awake () {
            player = Player.Get(GetComponent<ReferenceHub>());
            LastLevel = -2;
            buffer = new PlaybackBuffer(4096,endlessTapeMode:true);
            Proxy = null;
            hub.ServerSetEmotionPreset(DefaultPreset);
        }
	
        // Update is called once per frame
        void Update () {
            try
            {
                EmotionSubcontroller subcontroller;
                if (!(hub.roleManager.CurrentRole is IFpcRole currentRole) ||
                    !(currentRole.FpcModule.CharacterModelInstance is AnimatedCharacterModel
                        characterModelInstance) ||
                    !characterModelInstance.TryGetSubcontroller<EmotionSubcontroller>(out subcontroller))
                {
                    Destroy(this);
                    return;
                }
                
                if (_overrideEndTime > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                {
                    //Currently in an override state, do nothing
                    return;
                } else if (_overrideEndTime != 0)
                {
                    //Just finished an override, need to reset to default preset
                    _overrideEndTime = 0;
                    LastLevel = -2;
                }
                
                //Updated with speech volume
                if (/*!player.IsSpeaking*/DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()-LastPacketTime>500)
                {
                    //Player has released talk button, should close mouth if they weren't done so already
                    if (LastLevel != -1)
                    {
                        hub.ServerSetEmotionPreset(DefaultPreset);
                        LastLevel = -1;
                    }
                    
                }
                else
                {
                    //Player is attempting to speak, need to check how loud they currently are to determine how their mouth should behave
                    
                    float volume = CalculateRMSVolume();
                    float dbVolume = 20f * Mathf.Log10(volume);
                    
                    int level = 0;
                    if ( dbVolume < -80f)
                    {
                        level = 0;
                    }
                    else if (dbVolume >= -30f)
                    {
                        level = 2;
                    }
                    else
                    {
                        level = 1;
                    }

                    if (level != LastLevel)
                    {
                        LastLevel = level;
                        switch (level)
                        {
                            case 0:
                                hub.ServerSetEmotionPreset(EmotionPresetType.Neutral);
                                break;
                            case 1:
                                hub.ServerSetEmotionPreset(EmotionPresetType.Happy);
                                break;
                            case 2:
                                hub.ServerSetEmotionPreset(EmotionPresetType.Scared);
                                break;
                        }
                    }
                }

            } catch (Exception e)
            {
            }
        }
        
        public float CalculateRMSVolume()
        {
            float sumOfSquares = 0f;
            foreach(float sample in buffer.Buffer)
            {
                sumOfSquares += sample * sample;
            }
            return Mathf.Sqrt(sumOfSquares / buffer.Buffer.Length);
        }
        public void VoiceMessageReceived(byte[] data, int length)
        {
            if (Proxy != null)
            {
                if (!Proxy.ReferenceHub.TryGetComponent(out SpeechTracker tracker))
                {
                    return;
                }
                tracker.VoiceMessageReceived(data, length);
                return;
            }
            try
            {
                if (!(player.VoiceModule is HumanVoiceModule humanVoiceModule))
                {
                    return;
                }
                
                float[] samples = new float[1024]; //480
                int len = OpusDecoder.Decode(data, length, samples);
                buffer.Write(samples,len);
                LastPacketTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            } catch (Exception e)
            {
            }
        }
        public void OverrideEmotion(EmotionPresetType preset, int durationMs)
        {
            _overridePreset = preset;
            _overrideEndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + durationMs;
            hub.ServerSetEmotionPreset(_overridePreset);
        }
        
    }
}