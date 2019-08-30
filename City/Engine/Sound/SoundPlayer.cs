using System;

namespace City.Engine.Sound
{
    public class SoundPlayer : Object
    {

        FMOD.System soundSystem;

        public SoundPlayer(GameHandler game) : base(game)
        {
        }

        public SoundPlayer(GameHandler game, string Name) : base(game, Name)
        {
        }

        public override void Init()
        {
            base.Init();

            FMOD.Factory.System_Create(out soundSystem);
            soundSystem.init(1024, FMOD.INITFLAGS.NORMAL, (IntPtr)0);
        }

        public FMOD.Sound LoadSound(string path, FMOD.MODE mode = FMOD.MODE._2D)
        {
            FMOD.Sound sound;
            FMOD.RESULT res = soundSystem.createSound(path, mode, out sound);
            if (res == FMOD.RESULT.OK)
            {
                return sound;
            }
            else
            {
                throw new Exceptions.LoadFailException(FMOD.Error.String(res) + "    File name: " + path);
            }
        }

        public FMOD.Channel PlaySound(FMOD.Sound sound, FMOD.ChannelGroup group, bool paused = false)
        {
            FMOD.Channel channel;
            FMOD.RESULT res = soundSystem.playSound(sound, group, paused, out channel);
            if (res == FMOD.RESULT.OK)
            {
                return channel;
            }
            else
            {
                throw new Exceptions.LoadFailException(FMOD.Error.String(res));
            }
        }

        public FMOD.System SoundSystem { get => soundSystem; }
    }
}
