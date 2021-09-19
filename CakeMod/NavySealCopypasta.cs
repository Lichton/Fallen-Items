using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using BasicGun;
using System.Collections.Generic;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace CakeMod
{
    class NavySealCopypasta : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Talk-o-matic";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CakeMod/Resources/TalkOMatic";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<NavySealCopypasta>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Chatterbox";
            string longDesc = "A jury-rigged button normally found within the elevators of the gungeon, it's been made to prattle on when pressed.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 750);


            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
        }
        public IEnumerator DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration, bool isThoughtBubble)
        {
            if (isThoughtBubble)
            {
                TextBoxManager.ShowThoughtBubble(baseTransform.position + offset, baseTransform, duration, stringKey, false, true, GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag);
            }
            else
            {
                TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, duration, stringKey, GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
            }
            yield break;
        }
        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            System.Random r = new System.Random();
            int index = r.Next(TalkOMaticWords.Count);
            string randomString = TalkOMaticWords[index];
            int bighead = UnityEngine.Random.Range(1, 101);
            
            if (bighead == 100)
            {
                this.StartCoroutine(DoAmbientTalk(user.transform, new Vector3(0.75f, 1.5f, 0f), ("Get good, " + user.name + "."), 5f, false));
            }
            else if(bighead == 99)
            {
                this.StartCoroutine(DoAmbientTalk(user.transform, new Vector3(0.75f, 1.5f, 0f), user.name + ", eh? I've heard terrible things, my friend.", 5f, false));
            }
            {
                    this.StartCoroutine(DoAmbientTalk(user.transform, new Vector3(0.75f, 1.5f, 0f), randomString, 5f, false));
            }
        }

        List<string> TalkOMaticWords = new List<string>
        {
            "I live in constant agony.",
            "Getting hit? Just dodge, idiot.",
            "100% of fatalities in the gungeon are because of bullets. Don't get hit!",
            "Casings can be spent on goods and services!",
            "Do not hug the Gundead, they do not like it.",
            "Loading advice...",
           "Dodge rolling allows you to cross gaps you normally couldn't.",
           "Don't hoard your casings!",
           "Remember to use your blanks!",
           "Addiction can happen to anyone, keep your spice usage low.",
          "If you keep loosing health, try doing anything other then what you're doing.",
          "I've seen stumps with more motor control than you.",
          "Certain statues require blanks to activate.",
            "Guns with infinite ammo, like starter guns, don't reveal secret rooms.",
            "Get good.",
            "Dodging is not your only tool. Bob and weave between enemy bullets if you're in a pinch!",
            "Try a little harder, please.",
            "Stealing from shops can cause them to close them permenantly.",
            "Wow. You really suck at this.",
            "Shooting bullets makes fights shorter, combine shooting and dodging for optimal gunplay.",
            "Not dying leads to longer living, and therefore more looting.",
            "Some equipment is garbage, and others trash. Please know the difference.",
            "I'm not being passive aggressive. I'm just right.",
            "*laughter*",
            "I hope you encounter Fuselier on your trek through the Gungeon Proper.",
            "Don't break brown chests. They have a small chance to be hidden rainbow chests!",
            "F",
            "Watch out for that bullet!",
            "Use lockpicks on chests you don't care about before valuable ones.",
            "Unskilled lockpicking can lock you out of a floor or a room if it fails.",
            "Certain special equipment can stay with you into the past. Remember not to lose them!",
            "Blasphemy is a good weapon, but it can become a crutch. Be careful!",
            "Screw this, I'm out. Just kidding.",
            "I approximate your current chances of killing your past to be... not very good.",
"I approximate your current chances of killing your past to be... decent.",
"I approximate your current chances of killing your past to be... pretty good",
"Remember: The bullet kin are more scared of you than you are of them.",
"Do not drink the Gungeon's water.",
"The cake is a LIE!",
"Birds? A mere fallacy.",
"A dog is a gungeoneer's best friend.",
"Burning is bad. Don't do that.",
"The Resourceful Rat is really just lonely... probably.",
"Country roads, take me hooooooooome...",
            "Save your blanks for when the High Dragun throws his knives. A well timed blank can disintegrate them.",
            "If you find a bloody scarf on the ground, don't touch it.",
            "I'm hitting the stop button, but it just keeps on talking!",
            "Stop pressing my face!",
            "Error 404, quip not found.",
            "Download Prismatism! Wait, what?",
            "The directional pad has a few special inputs and will drop a chest when it runs out of ammo.",
            "You tellin' me a single scrawny little gungeoneer is the only thing between me and that Lich fellow?",
            "Just shoot once and take the ammo, fool!",
            "Poor accuracy is just a random chance to hit your target 100% of the time!",
            "Give my badge to my kids. Tell them I love them, too.",
            "How bout' you give me one of those guns and I'll show you how its done, eh?",
            "Sponsored by : Round King.",
            "The Last Chamber was cancelled.",
            "Alert! Red spy  in the base!",
            "I would KILL for a meat bun right now!"


        };

        protected override void OnPreDrop(PlayerController user)
        {

        }


        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return true;
        }
    }
}

