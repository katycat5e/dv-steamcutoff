using DvMod.HeadsUpDisplay;
using UnityEngine;
using UnityModManagerNet;

namespace DvMod.SteamCutoff
{
    class HeadsUpDisplayBridge
    {
        public static HeadsUpDisplayBridge instance;

        static HeadsUpDisplayBridge()
        {
            try
            {
                if (UnityModManager.FindMod("HeadsUpDisplay") == null)
                    return;
                instance = new HeadsUpDisplayBridge();
                instance.Register();
            }
            catch (System.IO.FileNotFoundException)
            {
            }
        }

        PushProvider steamGenerationProvider = new PushProvider(
            "Steam generation", () => true, v => $"{Mathf.RoundToInt(v * 1000)} mbar/s");
        PushProvider steamConsumptionProvider = new PushProvider(
            "Steam consumption", () => true, v => $"{Mathf.RoundToInt(v * 1000)} mbar/s");

        void Register()
        {
            Registry.Register(TrainCarType.LocoSteamHeavy, steamGenerationProvider);
            Registry.Register(TrainCarType.LocoSteamHeavy, steamConsumptionProvider);
        }

        public void UpdateSteamGeneration(TrainCar car, float pressureRise)
        {
            steamGenerationProvider.MixSmoothedValue(car, pressureRise);
        }

        public void UpdateSteamUsage(TrainCar car, float pressureDrop)
        {
            steamConsumptionProvider.MixSmoothedValue(car, pressureDrop);
        }

        public void UpdateCutoffSetting(TrainCar car, float cutoff)
        {
            ((PushProvider)Registry.GetProvider(car.carType, "Cutoff")).SetValue(car, cutoff);
        }
    }
}