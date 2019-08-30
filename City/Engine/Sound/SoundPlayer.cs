using Microsoft.Xna.Framework;
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
            soundSystem.init(1024, FMOD.INITFLAGS._3D_RIGHTHANDED, (IntPtr)0);
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

        public void Set3DListenerAttributes(int listenerId, Vector3 location, Vector3 velocity, Vector3 forward, Vector3 up)
        {
            //Vectors here are changed into fmod's one so that in future it will be easier to modify them
            FMOD.VECTOR FMODPos = new FMOD.VECTOR();
            FMODPos.x = location.X;
            FMODPos.y = location.Y;
            FMODPos.z = location.Z;

            FMOD.VECTOR FMODvel = new FMOD.VECTOR();
            FMODvel.x = velocity.X;
            FMODvel.y = velocity.Y;
            FMODvel.z = velocity.Z;

            FMOD.VECTOR FMODForw = new FMOD.VECTOR();
            FMODForw.x = forward.X;
            FMODForw.y = forward.Y;
            FMODForw.z = forward.Z;
            
            FMOD.VECTOR FMODUp = new FMOD.VECTOR();
            FMODUp.x = up.X;
            FMODUp.y = up.Y;
            FMODUp.z = up.Z;

            //Matrix.CreateTranslation(location).Up

            FMOD.RESULT res = soundSystem.set3DListenerAttributes(listenerId,ref FMODPos,ref FMODvel,ref FMODForw,ref FMODUp);
            if (res != FMOD.RESULT.OK)
            {
                throw new Exceptions.LoadFailException(FMOD.Error.String(res));
            }
        }

        /// <summary>
        /// Must be called in the end of every otehr update that affects sound
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            soundSystem.update();
        }
        public FMOD.System SoundSystem { get => soundSystem; }
    }
}
