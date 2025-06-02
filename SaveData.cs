using System;

namespace TamagotchiGame
{
    [Serializable]
    public class SaveData
    {
        public PetType PetType { get; set; }
        public string Name { get; set; }
        public int Hunger { get; set; }
        public int Happiness { get; set; }
        public int Health { get; set; }
        public int Cleanliness { get; set; }
        public int Energy { get; set; }
        public DateTime LastSaveTime { get; set; }
    }
}