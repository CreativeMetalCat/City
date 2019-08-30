using Microsoft.Xna.Framework;
using System;

namespace City.Engine.Sound
{
    public class SoundPlayer : Object
    {

        public static FMOD.VECTOR FMODVectorFromVector3(Vector3 vector)
        {
            FMOD.VECTOR res = new FMOD.VECTOR();
            res.x = vector.X;
            res.y = vector.Y;
            res.z = vector.Z;

            return res;
        }

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

        public FMOD.Reverb3D CreateReverb(FMOD.REVERB_PROPERTIES props, Vector3 location, float maxDist, float minDist)
        {
            FMOD.Reverb3D reverb;
            FMOD.RESULT res = soundSystem.createReverb3D(out reverb);
            if (res == FMOD.RESULT.OK)
            {
                res = reverb.setProperties(ref props);
                if (res == FMOD.RESULT.OK)
                {
                    FMOD.VECTOR pos = new FMOD.VECTOR();
                    pos.x = location.X;
                    pos.y = location.Y;
                    pos.z = location.Z;

                    res = reverb.set3DAttributes(ref pos, minDist, maxDist);
                    if (res == FMOD.RESULT.OK)
                    {

                        return reverb;
                    }
                    else
                    {
                        throw new Exceptions.LoadFailException(FMOD.Error.String(res));
                    }
                }
                else
                {
                    throw new Exceptions.LoadFailException(FMOD.Error.String(res));
                }
            }
            else
            {
                throw new Exceptions.LoadFailException(FMOD.Error.String(res));
            }
        }

        public void Set3DListenerAttributes(int listenerId, Vector3 location, Vector3 velocity, Vector3 forward, Vector3 up)
        {
            //Vectors here are changed into fmod's one so that in future it will be easier to modify them
            FMOD.VECTOR FMODPos = SoundPlayer.FMODVectorFromVector3(location);

            FMOD.VECTOR FMODvel = SoundPlayer.FMODVectorFromVector3(velocity);

            FMOD.VECTOR FMODForw = SoundPlayer.FMODVectorFromVector3(forward);

            FMOD.VECTOR FMODUp = SoundPlayer.FMODVectorFromVector3(up);

            //Matrix.CreateTranslation(location).Up

            FMOD.RESULT res = soundSystem.set3DListenerAttributes(listenerId, ref FMODPos, ref FMODvel, ref FMODForw, ref FMODUp);
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
