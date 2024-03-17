namespace RA2Mod.Survivors.Tesla.States
{
    public class DeployDoubleTeslaTowerScepter : DeployTeslaTower
    {
        private int towersLeft = 1;

        protected override void HandleConstructCoil()
        {
            //doesn't work. spams towers because we're checking skill.down instead of justpressed
            //scrapped anyways
            if (towersLeft > 0)
            {
                if (characterBody)
                {

                    constructCoil(currentPlacementInfo);

                    PlayCrossfade("Gesture, Override", "DoPlaceScepter", 0.1f);

                    towersLeft--;
                }
            }
            else
            {
                base.HandleConstructCoil();
            }
        }
    }
}