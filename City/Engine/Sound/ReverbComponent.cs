using FMOD;
using Microsoft.Xna.Framework;

namespace City.Engine.Components
{
    class ReverbComponent : Component
    {
        FMOD.REVERB_PROPERTIES reverbPropeties;
        FMOD.Reverb3D reverb;

        /// <summary>
        /// Location relative to actor
        /// </summary>
        public Vector3 location;

        readonly float minDist;
        readonly float maxDist;

        public ReverbComponent(GameHandler game, Actor owner, Vector3 location, FMOD.REVERB_PROPERTIES reverbPropeties, float minDist, float maxDist) : base(game, owner)
        {
            this.reverbPropeties = reverbPropeties;
            this.maxDist = maxDist;
            this.minDist = minDist;
        }

        public ReverbComponent(GameHandler game, string Name, Actor owner, Vector3 location, FMOD.REVERB_PROPERTIES reverbPropeties, float minDist, float maxDist) : base(game, Name, owner)
        {
            this.reverbPropeties = reverbPropeties;
            this.maxDist = maxDist;
            this.minDist = minDist;
        }

        public override void Init()
        {
            reverb = Game.soundPlayer.CreateReverb(reverbPropeties, location + owner.location, maxDist, minDist);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            FMOD.VECTOR pos = Engine.Sound.SoundPlayer.FMODVectorFromVector3(GetWorldLocation());
            if (reverb != null)
            {
                reverb.set3DAttributes(ref pos, minDist, maxDist);
            }
            else
            {
                throw new System.NullReferenceException("Attempt to work with null reverb in reverb component");
            }
        }

        public override Vector3 GetWorldLocation()
        {
            return location + owner.location;
        }

        public REVERB_PROPERTIES ReverbPropeties { get => reverbPropeties; set => reverbPropeties = value; }
        public Reverb3D Reverb { get => reverb; set => reverb = value; }
    }
}
