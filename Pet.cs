using System;
using System.Collections.Generic;

namespace TamagotchiGame
{
    // Enum for pet types
    public enum PetType
    {
        Cat,
        Dog,
        Robot
    }

    // Enum for pet states
    public enum PetState
    {
        Happy,
        Hungry,
        Sad,
        Sleeping,
        Normal
    }

    // Class representing the Tamagotchi pet
    public class Pet
    {
        public PetType Type { get; set; }
        public string Name { get; set; }
        public int Hunger { get; set; } = 50;
        public int Happiness { get; set; } = 50;
        public int Health { get; set; } = 100;
        public int Cleanliness { get; set; } = 50;
        public int Energy { get; set; } = 50;

        private Dictionary<PetState, string[]> animations;
        private int currentFrame = 0;

        public Pet(PetType type, string name)
        {
            Type = type;
            Name = name;
            InitializeAnimations();
        }

        private void InitializeAnimations()
        {
            animations = new Dictionary<PetState, string[]>();

            switch (Type)
            {
                case PetType.Cat:
                    animations[PetState.Normal] = new string[]
                    {
                        "    /\\_/\\  \n    ( o.o ) \n    > ^ <  \n     |   |/  \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( ^.^ ) \n    > - <  \n    |   |   \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( o.o ) \n    > v <  \n     |   |/  \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( -.^ ) \n    > ^ <  \n    |   |   \n    (|_ _|) "
                    };
                    animations[PetState.Happy] = new string[]
                    {
                        "    /\\_/\\  \n    ( ^.^ ) \n    > ^ <  \n    |   |  \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( ◕.◕ ) \n    > ^ <  \n    | ~ |  \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( ^.^ ) \n    > ^ <  \n    |   |  \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( ◕.◕ ) \n    > ^ <  \n    | ~ |  \n    (|_ _|) "
                    };
                    animations[PetState.Hungry] = new string[]
                    {
                        "    /\\_/\\  \n    ( >.< ) \n    > ω <  \n    |   |  \n      (|_ _|)__ ",
                        "    /\\_/\\  \n    ( T.T ) \n    > ω <  \n    | ~ |  \n     (|_ _|)/",
                        "    /\\_/\\  \n    ( >.< ) \n    > ω <  \n    |   |  \n      (|_ _|)__",
                        "    /\\_/\\  \n    ( @.@ ) \n    > ω <  \n    | ~ |  \n     (|_ _|)/"
                    };
                    animations[PetState.Sad] = new string[]
                    {
                        "    /\\_/\\  \n    ( ;.; ) \n    > ∩ <  \n    |   |  \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( T.T ) \n    > ∩ <  \n    |   |  \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( ;.; ) \n    > ∩ <  \n    |   |  \n    (|_ _|) ",
                        "    /\\_/\\  \n    ( T.T ) \n    > ∩ <  \n    |   |  \n    (|_ _|) "
                    };
                    animations[PetState.Sleeping] = new string[]
                    {
                        " zzz\n     /\\_/\\  \n    ( -.- ) \n    > ~ <  \n    |   |  \n    (|_ _|) ",
                        " Zzz\n     /\\_/\\  \n    ( -.= ) \n    > ~ <  \n    |   |  \n    (|_ _|) ",
                        " zZz\n     /\\_/\\  \n    ( -.- ) \n    > ~ <  \n    |   |  \n    (|_ _|) ",
                        " zzZ\n     /\\_/\\  \n    ( =.= ) \n    > ~ <  \n    |   |  \n    (|_ _|) "
                    };
                    break;

                case PetType.Dog:
                    animations[PetState.Normal] = new string[]
                    {
                        "  ^..^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  0..0     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^..^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  0..0     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\"
                    };
                    animations[PetState.Happy] = new string[]
                    {
                        "  ^^ ^^     ) \n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^◕ ◕^    ( \n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^^ ^^     ) \n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^◕ ◕^    ( \n /_/\\____/\n   /\\  /\\\n    /  \\/  \\"
                    };
                    animations[PetState.Hungry] = new string[]
                    {
                        "  ^> <^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^T T^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^@ @^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^> <^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\"
                    };
                    animations[PetState.Sad] = new string[]
                    {
                        "  ^; ;^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^T T^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^ಥ ಥ^    /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "  ^; ;^     /\n /_/\\____/\n   /\\  /\\\n    /  \\/  \\"
                    };
                    animations[PetState.Sleeping] = new string[]
                    {
                        "zzz\n  ^- =^    /  \n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "Zzz\n  ^= -^    /  \n /_/\\____/\n   /\\  /\\\n    /  \\/  \\",
                        "zZz\n  ^- -^    /  \n /_/\\____/\n   /\\  /\\\n    /  \\/  \\ ",
                        "zzZ\n  ^= =^    /  \n /_/\\____/\n   /\\  /\\\n    /  \\/  \\"
                    };
                    break;

                case PetType.Robot:
                    animations[PetState.Normal] = new string[]
                    {
                        "   ;\n   [\"\"]\n   /[_]\\\n  ] [\n",
                        "   ;\n   [oo]\n   /[_]\\\n  ] [\n",
                        "   ;\n   [^^]\n   /[_]\\\n  ] [\n",
                        "   ;\n   [\"\"]\n   /[_]\\\n  ] [\n"
                    };
                    animations[PetState.Happy] = new string[]
                    {
                        "   ;\n   [^^]♪♪♪\n   /[_]\\\n  ] [ \n",
                        "        ;\n   [◕◕]\n   /[_]\\\n  ] [ \n",
                        "   ;\n   [^^]♪♪♪\n   /[_]\\\n  ] [\n",
                        "        ;\n   [**]\n   /[_]\\\n  ] [ \n"
                    };
                    animations[PetState.Hungry] = new string[]
                    {
                        "   ;    \n      [><]!!!\n   /[_]\\\n  ] [ \n  ",
                        "   ;    \n   [XX]\n   /[_]\\\n  ] [ \n  ",
                        "   ;    \n      [><]!!!\n   /[_]\\\n  ] [ \n ",
                        "   ;    \n   [@@]\n   /[_]\\\n  ] [ \n "
                    };
                    animations[PetState.Sad] = new string[]
                    {
                        "   ;\n    [;;]\n   /[_]\\\n   ] [ \n ",
                        "   ;\n    [TT]\n   /[_]\\\n   ] [ \n",
                        "   ;\n    [;;]\n   /[_]\\\n   ] [ \n",
                        "   ;\n    [--]\n   /[_]\\\n   ] [\n"
                    };
                    animations[PetState.Sleeping] = new string[]
                    {
                        "zzz\n    ;\n    [--]\n   /[_]\\\n  ] [\n",
                        "Zzz\n    ;\n    [==]\n   /[_]\\\n  ] [\n",
                        "zZz\n    ;\n    [--]\n   /[_]\\\n  ] [\n",
                        "zzZ\n    ;\n    [..]\n   /[_]\\\n  ] [\n"
                    };
                    break;
            }
        }

        public string GetCurrentFrame()
        {
            PetState state = GetCurrentState();
            if (!animations.ContainsKey(state) || animations[state].Length == 0)
            {
                return animations[PetState.Normal][0];
            }
            string[] frames = animations[state];
            string frame = frames[currentFrame];
            currentFrame = (currentFrame + 1) % frames.Length;
            return frame;
        }

        public PetState GetCurrentState()
        {
            if (Energy < 20) return PetState.Sleeping;
            if (Hunger > 80) return PetState.Hungry;
            if (Happiness < 30 || Health < 30 || Cleanliness < 20) return PetState.Sad;
            if (Happiness > 70 && Hunger < 30 && Energy > 60) return PetState.Happy;
            return PetState.Normal;
        }

        public void Feed()
        {
            Hunger = Math.Clamp(Hunger - 30, 0, 100);
            Happiness = Math.Clamp(Happiness + 10, 0, 100);
            Energy = Math.Clamp(Energy + 5, 0, 100);
        }

        public void Play()
        {
            Happiness = Math.Clamp(Happiness + 20, 0, 100);
            Energy = Math.Clamp(Energy - 15, 0, 100);
            Hunger = Math.Clamp(Hunger + 10, 0, 100);
        }

        public void Heal()
        {
            Health = Math.Clamp(Health + 30, 0, 100);
            Energy = Math.Clamp(Energy - 10, 0, 100);
        }

        public void Clean()
        {
            Cleanliness = Math.Clamp(Cleanliness + 40, 0, 100);
            Happiness = Math.Clamp(Happiness + 5, 0, 100);
        }

        public void GoToSleep()
        {
            Energy = Math.Clamp(Energy + 70, 0, 100);
            Hunger = Math.Clamp(Hunger + 5, 0, 100);
            Happiness = Math.Clamp(Happiness + 10, 0, 100);
        }

        public void UpdateStats()
        {
            Hunger = Math.Clamp(Hunger + 2, 0, 100);
            Happiness = Math.Clamp(Happiness - 1, 0, 100);
            Cleanliness = Math.Clamp(Cleanliness - 1, 0, 100);

            PetState stateBeforeEnergyUpdate = GetCurrentState();
            if (stateBeforeEnergyUpdate == PetState.Sleeping)
            {
                Energy = Math.Clamp(Energy + 5, 0, 100);
            }
            else
            {
                Energy = Math.Clamp(Energy - 2, 0, 100);
            }

            if (Hunger > 80 || Happiness < 20 || Cleanliness < 10 || Energy < 10)
            {
                Health = Math.Clamp(Health - 3, 0, 100);
            }
            else if (Hunger < 50 && Happiness > 50 && Cleanliness > 50 && Energy > 30)
            {
                Health = Math.Clamp(Health + 1, 0, 100);
            }
        }

        public bool IsDead()
        {
            return Health <= 0;
        }
    }
}