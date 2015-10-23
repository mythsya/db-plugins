using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trinity.Cache
{
    public static class RiftProgression
    {
        // todo, either find formula that matches these values
        // or find way to map an actor sno/internalname/gbid to these real names.

        //Malthael	33.75
        //Adria	12.5
        //Greed	8.125
        //Tala	0
        //Garan	0
        //Aletur	0
        //Rakanoth	7.5
        //Azmodan	6.25
        //Cydaea	6.25
        //The Butcher	6.25
        //Samae	0
        //Korae	0
        //Man Carver	5
        //Ember	5
        //Bloodmaw	5
        //Maester Gnaeus	5
        //Perdition	5
        //Erethon	5
        //Raiziel	5
        //Blighter	5
        //Eskandiel	5
        //Agnidox	5
        //Orlash	5
        //Lord of Bells	5
        //Perendi	5
        //Infernal Maiden	5
        //Hamelin	5
        //{ph
        //    }
        //    Hed Monh Ton	0
        //{PH
        //}
        //Phil	0
        //Captured Barbarian  0
        //Saxtris	5
        //Voracity	5
        //Bone Warlock    5
        //Ghom	5
        //Dirgest	5
        //The Vile Executioner	5
        //The Undying One	5
        //The Soul of Evil    5
        //Chief Elder Kanai	0
        //The Savage Behemoth	5
        //The Merciless Witch	5
        //The King of the Dead	5
        //The Foul Desecrator	5
        //The Cow Queen	5
        //The Choker  5
        //The Binder  5
        //Tethrys	5
        //Cold Snap   5
        //Maghda	5
        //Fenring	5
        //Crusader King   5
        //Rime	5
        //Stonesinger	5
        //Skular	0
        //Sand Shaper 5
        //Maester Licinius    5
        //Maester Julius  5
        //Odeg the Keywarden	4.75
        //Kulle	4.5
        //Arthak	0
        //Aspect of Anguish	3.75
        //Aspect of Destruction	3.75
        //Aspect of Hatred	3.75
        //Aspect of Lies	3.75
        //Aspect of Pain	3.75
        //Peleil	3.75
        //Aspect of Sin	3.75
        //Mulliuqs the Horrid	3.75
        //Zoltun Kulle    3.75
        //Realmwalker	3.75
        //Kurekas	3.75
        //Captain Shely   3.375
        //Emikdeva	3.375
        //Tarthess	3.25
        //Catharis	3.25
        //Drygha	3.25
        //Gugyn the Gauntlet	3.125
        //Arcuss	3.125
        //Lianthe	3.125
        //Lu'ca	3.125
        //Lucious the Depraved	3.125
        //Sarella the Vile	3.125
        //Shertik the Brute	3.125
        //Vekriss	3.125
        //Arsect the Venomous	3.125
        //Vek Tabok   3.125
        //Arthur	3.125
        //Vek Marru   3.125
        //Ashangu	3.125
        //Ashek	3.125
        //Vek Dahyrn  3.125
        //Lacocious the Diseased	3.125
        //Mange	0
        //Knasher the Depraved	3.125
        //Gryssian	3.125
        //Judge Alzarius  3.125
        //Axegrave the Executioner	3.125
        //Axgore the Cleaver	3.125
        //Valtesk the Cruel	3.125
        //Gruewl	3.125
        //Baethus	3.125
        //Sharpclaw	3.125
        //Ballartrask the Defiler	3.125
        //Balzhak	3.125
        //Growlfang	3.125
        //Valoelle	3.125
        //Growler	3.125
        //Grotescor	3.125
        //Bari Bairi  3.125
        //Bari Hattar 3.125
        //Bari Moqqu  3.125
        //Valifahr the Noxious	3.125
        //Barrucus	3.125
        //Barty the Minuscule	3.125
        //Bashface the Truncheon	3.125
        //Bashiok	3.125
        //Malthous	3.125
        //Grodeloth	3.125
        //Shandra'Har	3.125
        //Uzkez the Annihilator	3.125
        //Belagg Pierceflesh  3.125
        //Groak the Brawler	3.125
        //Bellybloat the Scarred	3.125
        //Belphegor	3.125
        //Grimsmack	3.125
        //Bernael	3.125
        //Grimnight the Soulless	3.125
        //Beyatt	3.125
        //Bholen	3.125
        //Pontius	3.125
        //Kysindra the Wretched	3.125
        //Blarg the Foul	3.125
        //Blarg the Imp	3.125
        //Urgrek the Bodger	3.125
        //Greelode the Unforgiving	3.125
        //Gravitus	3.125
        //Grand Maester   3.125
        //Gozmol	3.125
        //Blightrot	0
        //Goz'turr the Torturer	3.125
        //Lord Kertis 3.125
        //Gout Foot Johnson	3.125
        //Dervish Lord    3.125
        //Troven	3.125
        //Gorog the Bruiser	3.125
        //Kago the Cunning	3.125
        //Gormungandr	3.125
        //Purah	3.125
        //Gobbulard	3.125
        //Gnawbone	3.125
        //Pyres the Damned	3.125
        //Bloodfeather	3.125
        //Bloone	3.125
        //Blorth	3.125
        //Bludgeonskull the Mauler	3.125
        //Glidewing	3.125
        //Tridiun the Impaler	3.125
        //Pyresh	3.125
        //Ghrentuloth	3.125
        //Trejiak	3.125
        //Treefist Woodhead   3.125
        //Bonefire	3.125
        //Koldozoor	3.125
        //Boneslag the Berserker	3.125
        //Bonesplinter	3.125
        //Borgoth	3.125
        //Kalmor	3.125
        //Sawtooth	3.125
        //Braluk Grimlow  3.125
        //Bramok the Overlord	3.125
        //Lummock the Brute	3.125
        //Quorel	3.125
        //Qurash the Reviled	3.125
        //Rad'noj	3.125
        //Khaaz	3.125
        //Torsar	3.125
        //Lurk	3.125
        //Bricktop	3.125
        //Khahul the Serpent	3.125
        //Khatun	3.125
        //Torchlighter	3.125
        //Brimstone	3.125
        //Ragus Grimlow   3.125
        //Seraziel	3.125
        //Gholash	3.125
        //Brother Amorth  0
        //Scorpitox	3.125
        //Kao' Ahn	3.125
        //Brother Buckley 3.125
        //Kapra	3.125
        //Lord Darius Brenden	3.125
        //Brother Hershberg   3.125
        //Dieyno the Warlock	3.125
        //Raiha the Vicious	3.125
        //Lord Brone  3.125
        //Ghallem the Cruel	3.125
        //Brutu	3.125
        //Getzlord	3.125
        //Thugeesh the Enraged	3.125
        //Thrum	3.125
        //Buras the Impaler	3.125
        //Burrask the Tunneler	3.125
        //Digger O'Dell	3.125
        //Thromp the Breaker	3.125
        //Busaw	3.125
        //Direclaw the Demonflyer	3.125
        //Cadhul the Deathcaller	3.125
        //Kube	3.125
        //Katherine Batts 3.125
        //Thornback	3.125
        //Thilor	3.125
        //Krusk	3.125
        //Gart the Mad	3.125
        //Captain Cage    3.125
        //Captain Clegg   3.125
        //Captain Dale    3.125
        //Brakan	3.125
        //Captain Donn Adams	3.125
        //Captain Gerber  3.125
        //Maiden of Flame	3.125
        //Captain Tollifer    3.125
        //Logrut the Warrior	3.125
        //Keeper Hashemel 3.125
        //Ennyo the Warlock	3.125
        //Korchoroth	3.125
        //Therk	3.125
        //Garrach the Afflicted	3.125
        //Carrolanne	3.125
        //Garganug	3.125
        //Battlerage the Plagued	3.125
        //Theodyn Deathsinger 3.125
        //Theodosia Buhre 3.125
        //The Wickerman   3.125
        //The Warden  3.125
        //Horrus the Nightstalker	3.125
        //Celik	3.125
        //Lythan the Exiled	3.125
        //Esiel	3.125
        //The Tomekeeper  3.125
        //Semyaza the Dark	3.125
        //Pemphrido the Warlock	3.125
        //Charger	3.125
        //Charuch the Spear	3.125
        //Wargiant	3.125
        //Penny Lootbottoms   3.125
        //Percepeus	3.125
        //The Old Man	3.125
        //Chiltara	3.125
        //High Cultist Murdos	3.125
        //Hemit the Feared	3.125
        //Hellscream	3.125
        //The Flew    3.125
        //The Crusher 3.125
        //Wahr	3.125
        //The Cow Princess	3.125
        //Elgorath	3.125
        //Ganthar the Trickster	3.125
        //Phyneus the Growler	3.125
        //Galush Valdant  3.125
        //Funerus	3.125
        //Chupa Khazra    3.125
        //Cimeries	3.125
        //Erra	3.125
        //Magnar	3.125
        //Clawgane	3.125
        //Rathlin the Widowmaker	3.125
        //The Archivist   3.125
        //Sao'Thall	3.125
        //M'Dukkan	3.125
        //Templeton	3.125
        //Allucayrd	3.125
        //Friedrich Bartholomew   3.125
        //Razael the Feared	3.125
        //Almash the Grizzly	3.125
        //Colossal Firewing   3.125
        //Aloysius the Ghastly	3.125
        //Lograth	3.125
        //Red Rock    3.125
        //Reggrel the Despised	3.125
        //Scar Talon  3.125
        //Kresh	3.125
        //Fohsgrave The Usurper	3.125
        //Firestarter	3.125
        //Revelor	3.125
        //Krailen the Wicked	3.125
        //Tartus	3.125
        //Amduscias	3.125
        //Taros the Wild	3.125
        //Maggrus the Savage	3.125
        //Maggotburst	3.125
        //Crabbs	3.125
        //Crassus the Tormentor	3.125
        //Scythys	3.125
        //Targerious	3.125
        //Rhau'Kye	3.125
        //Plagar the Damned	3.125
        //Shondar the Invoker	3.125
        //Fecklar's Ghost	3.125
        //Mage Lord Skomara	3.125
        //Dragus	3.125
        //Hedros	3.125
        //Marchocyas	3.125
        //Cudgelarm	3.125
        //Tadardya	3.125
        //Cultist Grand Inquisitor	3.125
        //Emberwing	3.125
        //Fangbite	3.125
        //Josh Mosqueira  3.125
        //Super Awesome Sparkle Cake  3.125
        //Riplash	3.125
        //Rockgut	3.125
        //Mage Lord Misgen	3.125
        //Volux the Forgotten	3.125
        //Krelm the Flagitious	3.125
        //Dale Hawthorne  3.125
        //Mage Lord Ghuyan	3.125
        //Sumaryss the Damned	3.125
        //Hazzor the Viper	3.125
        //Dreadclaw the Leaper	3.125
        //Rockmaw	3.125
        //Ssthrass	3.125
        //Dreadgrasp	3.125
        //Sprynter	3.125
        //Rogoth the Eternal	3.125
        //Sabnock The Infector	3.125
        //Killian Damort  3.125
        //Saha the Slasher	3.125
        //Mage Lord Flaydren	3.125
        //Saint Tiffany   3.125
        //Vilepaw	3.125
        //Samaras the Chaser	3.125
        //Sammash	3.125
        //Mage Lord Caustus	3.125
        //Sotnob the Fool	3.125
        //Sorenae	3.125
        //Son of Jacob	3.125
        //Drury Brown 3.125
        //Snoglatch Grimwield 3.125
        //Snitchley	3.125
        //Eremiel	3.125
        //Sloggon	3.125
        //Slinger	3.125
        //Sledge	3.125
        //Dataminer	3.125
        //Slarth the Tunneler	3.125
        //Slarg the Behemoth	3.125
        //Erdith	3.125
        //Skewilliam	3.125
        //Evil Oliver 3.125
        //Death Maiden    3.125
        //Evan the Terrible	3.125
        //Demonika the Wicked	3.125
        //Skehlinrath	3.125
        //Skeetch the Hoarder	3.125
        //Severclaw	3.125
        //Haxxor	3.125
        //Hawthorne Gable 3.125
        //Simurgh	3.125
        //Haures	3.125
        //Severag	3.125
        //Hammermash	3.125
        //Hrugowl the Defiant	3.125
        //Ankou	3.125
        //Pearsing Son    3.125
        //Pazuzu	3.125
        //Paolillus	3.125
        //Pan Fezbane 3.125
        //Hyadures	3.125
        //Hyrug the Malformed	3.125
        //Vicious Gray Turkey	3.125
        //Igor Stalfos    3.125
        //Imgurr	3.125
        //Veshan the Fierce	3.125
        //Obis the Mighty	3.125
        //Oah' Tash	3.125
        //Ningish	3.125
        //Lord Poirier    3.125
        //Antinayl the Hidden	3.125
        //Nine Toads  3.125
        //Nikkorax	3.125
        //Hamish	3.125
        //Inquisitor Hamath   3.125
        //Larvus	3.125
        //Nalghban the Foul	3.125
        //Nak Sarugg  3.125
        //Nak Qujin   3.125
        //Nak Brekthar    3.125
        //Mundunogo	3.125
        //Verlix	3.125
        //Mourdred	3.125
        //Lashtongue	3.125
        //Morghum the Beast	3.125
        //Jebb	3.125
        //Jezeb the Conjuror	3.125
        //Jhorum the Cleric	3.125
        //Mistress Julia  3.125
        //Minra	3.125
        //Micheboar	3.125
        //John Gorham Coffin	3.125
        //Miasma	3.125
        //Mhawgann the Unholy	3.125
        //Merrium Skullthorn  3.125
        //Melmak the Swift	3.125
        //Mekhare	3.125
        //Mehshak the Abomination	3.125
        //Jonathan Muddlemore 3.125
        //Matanzas the Loathsome	3.125
        //Watareus	3.125
        //Xaphane	3.125
        //Xolotl	3.125
        //Xoren	3.125
        //Yakara	3.125
        //Yellow Ledbiter 3.125
        //Yergacheph	3.125
        //Yeth	3.125
        //Yomeeh	3.125
        //Yorkathraxx	3.125
        //Zhelobb the Venomous	3.125
        //Lorzak the Powerful	3.125
        //Zorrus	3.125
        //[PH]
        //FloaterAngel Benchmark Thunderstorm	0
        //[TEMP]
        //Fat Grub 3.125
        //{ph} Ancient Lich   0
        //Gwargumm the Befouled	3.125
        //Venimite	3.125
        //{ph} Primal Wildwood    0
        //Amok the Breaker	3
        //Charok	2.875
        //Kasadya	2.875
        //Ezek	2.8125
        //Aspect of Terror	2.8125
        //Hakkajar	2.75
        //Ernutet	2.5
        //Eternal Guardian    2.5
        //Ezek the Prophet	2.5
        //Father Rathe    2.5
        //Fearby the Prowler	2.5
        //Fezuul	2.5
        //Fire Burst  2.5
        //Flesh of Nar Gulle  2.5
        //Maahes	2.5
        //Razormouth	2.5
        //Razorclaw	2.5
        //Raziel	2.5
        //Rayeld	0
        //Frost Sentinel  2.5
        //Raskshak	2.5
        //Fuad the Cannibal	2.5
        //Gavin the Thief	2.5
        //Gharbad the Strong	2.5
        //Ghas	2.5
        //Ghezrim	2.5
        //Kurel	2.5
        //R'Lyeh	2.5
        //Quentin Sharpe  0
        //Princess Stardust   2.5
        //Graveljaw the Devourer	2.5
        //Grool	2.5
        //Little Jebby Rathe	2.5
        //Handible	2.5
        //Haziael	2.5
        //Lord Wynton 2.5
        //Hooded Corruptor    2.5
        //Aaron Bright    2.5
        //Lady Victoria   2.5
        //Marko	2.5
        //Vu the Destroyer	2.5
        //Vosk	2.5
        //Larel	2.5
        //Ancient Guardian    2.5
        //Virgil Cutthroat    2.5
        //Hurax	2.5
        //Manning the Monstrous	2.5
        //Vile Sentinel   2.5
        //Otzi the Cursed	2.5
        //Latimorr Edgars 2.5
        //Manglemaw	2.5
        //Lavarinth	2.5
        //Armorer's Bane	2.5
        //Avatar of Rakanishu	2.5
        //Balata	2.5
        //Caretaker McCree    2.5
        //Bertram Thistwhistle    2.5
        //Tubbers	2.5
        //Malgash	2.5
        //Boneripper	2.5
        //Bradshaw the Behemoth	2.5
        //Nightmarity	2.5
        //Nigel Cutthroat 2.5
        //Tortured Soul   2.5
        //Larson the Strange	2.5
        //Iskatu	2.5
        //Kamyr	2.5
        //Ixatla	2.5
        //Izual	2.5
        //Nekarat the Keywarden	2.5
        //Jacobs the Gigantic	2.5
        //Nashok	2.5
        //Tormented Behemoth  2.5
        //Lord Dunhyld    2.5
        //Malfeas the Abhorrent	2.5
        //Brother Karel   2.5
        //Burrow Bile 2.5
        //Buttercup	2.5
        //Lord Mathieu    2.5
        //Maisie the Daisy	2.5
        //Mother Rathe    2.5
        //Morris Jacobs   2.5
        //Leodesh The Stalker	2.5
        //Maiden Lamiel   2.5
        //Jay Wilson  2.5
        //Thieves Guild Summoner	2.5
        //Mordrath	2.5
        //Moontooth Dreadshark    2.5
        //Monstrous Dune Thresher	2.5
        //Molten Sentinel 2.5
        //Charged Sentinel    2.5
        //TEMP-Fallen Angel Voltron-TEMP	2.5
        //Moggra	2.5
        //Moek	2.5
        //Target Dummy(Level 5)  2.5
        //Miss Hell   2.5
        //Crazed Man  0
        //Minaca	2.5
        //Midnight Sparkle    2.5
        //Creampuff	2.5
        //Cultist Reanimator  2.5
        //Stinging Death Swarm	2.5
        //Dargon	2.5
        //Spirit of Khan Dakab    2.5
        //Sokahr the Keywarden	2.5
        //Snapbite	2.5
        //Sister Lysa 2.5
        //Simon's Note	2.5
        //Jondar	2.5
        //Maulin Sorely   2.5
        //Simon Cutthroat 2.5
        //Sicklefang	2.5
        //Xah'Rith the Keywarden	2.5
        //Shogg	0
        //Shatterbone	2.5
        //Shanabi	2.5
        //Shaitan the Broodmother	2.5
        //Diablo	2.5
        //Killaire	2.5
        //Shade of Nar Gulle  2.5
        //{PH}MerQueenie	0
        //Koghar	2.5
        //Zanthek	2.5
        //Zelusa the Grasping	2.5
        //Ebenezer Samuel 2.5
        //Ekthul	2.5
        //Scarnax	2.5
        //[PH]
        //CreepMob Benchmark Arcane	0
        //Enkidu	2.5
        //[PH]
        //LacuniFemale Benchmark Illusionist	0
        //Enmerkar	2.5
        //Enoch White 2.5
        //Erach	2.5
        //Erelus	2.5
        //Sardar	2.5
        //Rejakka	2.375
        //Urik The Seer	2.25
        //Urzel Mordreg   2.25
        //Lasciate	2.25
        //Shadow Clone    2.25
        //Aquinos	2.25
        //Stanley,Sinister Coven Administrator	2.125
        //Anton Embersoul 2.125
        //Rekkar	2.125
        //{ph} Mad Den Mother	0
        //Chancellor Eamon    2.125
        //Sartor	2
        //Magrethar	2
        //Soul Scavenger  2
        //Sarkoth	2
        //The Beast   2
        //Warrior	2
        //Banished Firewing   2
        //Barg the Elder	2
        //Rockulus	2
        //Gorathra	2
        //Obsidious	2
        //Headcleaver	1.875
        //Captain Daltyn  1.875
        //Wretched Queen  1.875
        //Brother Moek    1.875
        //Brother Larel   1.875
        //Mira Eamon  1.875
        //Farnham	1.875
        //Zalud	1.75
        //Malevolent Tormentor    1.625
        //Gelatinous Sire 1.625
        //Gelatinous Spawn    1.625
        //Reprehensible Fiend 1.625
        //Gem Hoarder 1.625
        //Insufferable Miscreant  1.625
        //Treasure Fiend  1.625
        //Treasure Goblin 1.625
        //Treasure Goblin Helper	1.625
        //Odious Collector    1.625
        //Rainbow Goblin  1.625
        //Gilded Baron    1.625
        //The Chiseler    1.625
        //Herald to the Queen 1.625
        //Blood Thief 1.625
        //Gorrel	1.625
        //Brickmaw	0
        //{PH} Leviathan	0
        //[PH]
        //Brickhouse Benchmark Frozen	0
        //Mawdol	0
        //Demonic Hell Bearer	1.5
        //Talos	0
        //Lloigor the Crazed	1.5
        //Korbal	0
        //Punisher	1.25
        //Executioner	1.25
        //Great Horned Goliath	1.25
        //Timelost Demon Soldier	1.25
        //[PH]
        //BigRed Benchmark Jailer	0
        //Exhumed	1.25
        //Maggot Brood    1.25
        //Horned Charger  1.25
        //Bogan Tower 1.25
        //Disentombed Hulk    1.25
        //Ironsmith Maldonado 1.25
        //Mallet Lord 1.25
        //Wooly Beast 1.25
        //Belial	1.25
        //Flesh Shaman    1.25
        //Glacial Colossus    0
        //Venomous Spinner    1.125
        //Glacial Monstrosity 0
        //Shocking Crawler    1.125
        //Rat-Infested Corpses    1.125
        //Rat King    0.875
        //{ph} Lupin	0
        //Unburied	1
        //Uninterred	1
        //Hive Cluster    1
        //Frozen Berserker    1
        //Molten Berserker    1
        //Frost Lurker    1
        //Scorching Creeper   1
        //Skeletal Beast  1
        //Corpse Pile 1
        //Savage Beast    1
        //Dread Stalker   0.95
        //Matron of Death	0.9375
        //[TEMP]
        //PVP melee    0.875
        //{PH} Bone Breaker   0
        //The Spy 0.875
        //{PH} Hive Mother    0
        //Talic	0.875
        //Bloated Malachor    0.875
        //{PH} Silverback	0
        //Colossal Golgor 0.875
        //Fharzula	0
        //Korlic	0.875
        //Mawdawc	0.875
        //Tusked Bogan    0.875
        //Hound Pack Leader	0.875
        //Hellhide Tremor 0.875
        //{ph} Shuddering Mass    0
        //Maniacal Golgor 0.875
        //Demonic Tremor  0.875
        //Diseased Bogan  0.875
        //[TEMP]
        //PVP_ranged	0.8125
        //Carrion Nest    0.75
        //Fallen Overseer 0.75
        //Gatekeeper	0.75
        //Summoner of the Dead    0.75
        //Rock Giant  0.75
        //Summoning Orb   0.75
        //Fallen Slavelord    0.75
        //Cursed Nest 0.75
        //Sewer Drain 0.75
        //Scarab Cocoon   0.75
        //Broiling Obelisk    0.75
        //Corrupt Hellbreeder 0.75
        //Summoner of Destruction	0.75
        //Endless Spawner 0.75
        //Ghostly Fallen Champion	0.75
        //Wood Wraith 0.75
        //Sentient Obelisk    0.75
        //Templar Inquisitor  0.75
        //{PH} Wildwood Bulwark   0
        //Blood Nest  0.75
        //Diseased Bodies 0.75
        //[TEMP]
        //Sand Monster 0.75
        //Primordial Scavenger    0.625
        //Demonic Slave   0.75
        //Arctic Obelisk  0.75
        //Arcane Construct    0.75
        //Half-eaten Bodies   0.75
        //Halissa	0.75
        //Plague Nest 0.75
        //Ancient Walker  0.75
        //Activated Pillar    0.75
        //Warcrazed Angel 0.75
        //Sand Twister    0.75
        //Highland Walker 0.75
        //Siegebreaker_Skeleton_Spawner	0.75
        //Sand Dweller    0.75
        //Skeleton Archer 0.75
        //Sand Behemoth   0.75
        //Fallen Commander    0.75
        //Fallen Legionnaire  0.75
        //Fallen Master   0.75
        //Fallen Overlord 0.75
        //Improved Risen Servitor	0.6875
        //Malefactor Vephar   0.625
        //Witch Doctor's Fetish D	0.625
        //Exorcist	0.5
        //Armaddon	0.625
        //Plagued Creeper 0.625
        //Wolf Companion  0.625
        //Mounted Armaddon    0.625
        //Siege Hook  0.625
        //Tormented Thrall    0.625
        //Toxic Lurker    0.625
        //Eldwin	0.625
        //Queen Araneae   0.625
        //Pyrogeist	0.625
        //Boar Companion  0.625
        //Baby Murloc 0.625
        //Lachdanan's Ghost	0.625
        //Ferret Companion    0.625
        //Witch Doctor's Fetish A	0.625
        //Spider Companion    0.625
        //Mass Confusion - Spirit	0.625
        //Raven Companion 0.625
        //Companion	0.625
        //Templar Enforcer    0.625
        //Inquisitor Sebacious    0.625
        //Bat Companion   0.625
        //[temp]
        //Bog Croc 0.625
        //Bogan Trapper   0.5
        //Isendra	0.625
        //Skeleton King   0.625
        //Templar of the Order    0.625
        //Lift Operator   0.625
        //Palerider Beleth    0.625
        //Binkles the Frog	0.625
        //Unholy Thrall   0.625
        //Malignant Thrall    0.625
        //Dark Moon Clan Ghost    0.625
        //Gargantuan	0.5625
        //Timelost Marauder   0.5625
        //Fiery Beast 0.5625
        //Corpse Raiser   0.5
        //Thrall of Maghda	0.5
        //Thrall of the Witch 0.5
        //Zao`Kapazao	0.5
        //Ghastly Seraph  0.5
        //Vengeful Seraph 0.5
        //TEMP Core Elite Demon Fancy	0.5
        //Dark Berserker  0.5
        //Fallen Hellhound    0.5
        //Fallen Hound    0.5
        //Dune Dervish    0.5
        //Warping Horror  0.5
        //Fallen Mongrel  0.5
        //Sand Dervish    0.5
        //Pain Monger 0.5
        //Lacuni Warrior  0.5
        //Canine Bones    0.5
        //Murderous Thug  0.5
        //Warcrazed Arbalest  0.5
        //Vicious Magewraith  0.5
        //Imprisoned Anarch   0.375
        //Shade of Malthael	0.5
        //Angry Cow   0.5
        //Rat Caller  0.5
        //Hell Basher 0.5
        //Chubby Purple Unicorn	0.5
        //Vicious Mangler 0.5
        //Chubby Pink Unicorn	0.5
        //Anarch	0.375
        //Hulking Phasebeast  0.5
        //Fallen Cur  0.5
        //Swift Flesheater    0.5
        //Wintersbane Stalker 0.5
        //Lacuni Slasher  0.5
        //Timelost Angel  0.45
        //Horror	0.4375
        //Morlu Inferno   0.375
        //Morlu Incinerator   0.375
        //Herald of Pestilence	0.375
        //Harvester	0.4375
        //Emberdread	0.4375
        //Warscarred Ravager  0.4375
        //Witchdoctor_PitOfFire_fetish	0.4375
        //Portal	0.4375
        //Possessed	0.4375
        //Praetorian Charger  0.4375
        //Praetorian Lobber   0.4375
        //Abomination	0.4375
        //Grotesque	0.4375
        //Wandering Tinker    0.375
        //Warden	0.375
        //Dust Eater  0.5
        //Withermoth	0.375
        //Sand Tremor 0
        //Spectral Khazra Shaman	0.375
        //Oppressor	0.375
        //Dark Moon Clan Shaman   0.375
        //Frederick	0.375
        //Larra	0.375
        //Rancid Stumbler 0.5
        //Horace	0.375
        //Rockworm	0.375
        //Spirit of Malthael	0.375
        //Rotting Corpses 0.375
        //Blood Clan Warrior	0.375
        //Veiled Sentinel 0.375
        //Blood Destroyer 0.375
        //Avatar of the Order 0.375
        //Purple Rainbow Unicorn	0.375
        //Savage Rockworm 0.375
        //Barbed Lurker   0.375
        //Researcher	0.375
        //Voice of the Prophet    0.375
        //Servant of Izual	0.375
        //Enchantress	0.375
        //Urzael	0.375
        //Soulless Shambler   0.5
        //Templar	0.375
        //Terror Demon    0.375
        //Dune Thresher   0.375
        //Margaret	0.375
        //Hellbound Brute 0.375
        //Demonfire Nightmare 0.375
        //Demonic Serpent 0.375
        //Arachnid Horror 0.375
        //Scavenging Tunneler 0.375
        //Flesh Gorger    0.375
        //TEMP Big Red	0.375
        //Warscarred Marauder 0.375
        //Pink Rainbow Unicorn	0.375
        //Walking Corpse  0.5
        //Blood Clan Mauler	0.375
        //Fetish Army 0.375
        //Triune Wizard   0.375
        //Prisoner's Remains	0.375
        //Imprisoned Oppressor Spirit	0.3375
        //Despairing Guard    0.3125
        //Desperate Guard 0.3125
        //Despina	0.3125
        //Diadra the Scholar	0.3125
        //Drian	0.3125
        //Dungeon Guard   0.3125
        //Dungeon Warden  0.3125
        //Dying Villager  0.3125
        //Edric	0.3125
        //Elayne	0.3125
        //Enchant	0.3125
        //Enkasi	0.3125
        //Fetish Sycophant    0.3125
        //Fevered Noble   0.3125
        //Forge Armor 0.3125
        //Forge Weapons   0.3125
        //Former Mayor Holus	0.3125
        //Forward Lieutenant Rial	0.3125
        //Frightened Girl 0.3125
        //Gareth	0.3125
        //Gem Combiner    0.3125
        //General Torion  0.3125
        //Gerard	0.3125
        //Ghost of Gharbad	0.3125
        //Ghost of Halbu	0.3125
        //Ghost of Jamella	0.3125
        //Ghostly Servitor    0.3125
        //Gorell the Quartermaster	0.3125
        //Griffyth the Warmaster	0.3125
        //Guard Benton    0.3125
        //Guard Findal    0.3125
        //Guard of the Keep   0.3125
        //Guard Sarth 0.3125
        //Guard Trent 0.3125
        //Hadley	0.3125
        //Haedrig Eamon   0.3125
        //Halmin the Alchemist	0.3125
        //Hermit	0.3125
        //Hopeless Guard  0.3125
        //Hugh	0.3125
        //Hungering Bones 0.3125
        //Ice Clan Warrior	0.3125
        //Inarius	0.3125
        //Infernal Bovine 0.3125
        //Iron Wolf   0.3125
        //Itherael	0.3125
        //Ivy	0.3125
        //Janett	0.3125
        //Javad the Merchant	0.3125
        //Joshua	0.3125
        //Jotham	0.3125
        //Kadala	0.3125
        //Kanai's Cube	0
        //Karla	0.3125
        //Karyna	0.3125
        //Keep Guard  0.3125
        //Keep Recruit    0.3125
        //King Justinian IV	0.3125
        //Krist	0.3125
        //Kyr the Weaponsmith	0.3125
        //Lady Dunhyld    0.3125
        //Lady Serena 0.3125
        //Lesser Soul Seeker	0.3125
        //Lieutenant Clyfton  0.3125
        //Lieutenant Gryffith 0.3125
        //Lieutenant Lavail   0.3125
        //Lieutenant Mentyn   0.3125
        //Litton the Fence	0.3125
        //Lorath Nahr 0.3125
        //Lord Darryl 0.3125
        //Lord Dewhurst   0.3125
        //Lord Harold Snowe	0.3125
        //Lord Vermilion  0.3125
        //Lost Angel  0.3125
        //Lost Soul   0
        //Lugo the Miner	0.3125
        //Maida	0.3125
        //Marlow	0.3125
        //Mayor Holus 0.3125
        //Merhan	0.3125
        //Messenger Martyns   0.3125
        //Moon Clan Ghost	0.3125
        //Moon Clan Warrior	0.3125
        //Mortally Wounded Guard	0.3125
        //Myriam	0.3125
        //Mystic's Cauldron	0.3125
        //Nephalem Spirit 0.3125
        //Nikola	0.3125
        //Nurse Melana    0.3125
        //OmniNPC_Tristram_Male_C_NoWander	0.3125
        //Orek	0.3125
        //Oswyn	0.3125
        //Patrol Chief    0.3125
        //Penny Bartholomew   0.3125
        //Powell the Miner	0.3125
        //Priest	0.3125
        //Prisoner	0.3125
        //Private James   0.3125
        //Qilah the Armorer	0.3125
        //Queen Asylla    0.3125
        //Radek the Fence	0.3125
        //Rakkis	0.3125
        //Remove Gem  0.3125
        //Returned Executioner    0.3125
        //Revenant Shield Guard	0.3125
        //Rickard	0.3125
        //Risen Bones 0.3125
        //Rodger the Alchemist	0.3125
        //Rondal	0.3125
        //Ruthie the Fence	0.3125
        //Sadeir the Innkeeper	0.3125
        //Salvage	0.3125
        //Sergeant Brooks 0.3125
        //Sergeant Burroughs  0.3125
        //Sergeant Dalen  0.3125
        //Sergeant Pale   0.3125
        //Sergeant Samuels    0.3125
        //Severin	0.3125
        //Siege Angel Astrius	0.3125
        //Siege Angel Elothael	0.3125
        //Silmak the Fence	0.3125
        //Skeletal Executioner    0.3125
        //Skeletal Reaper 0.3125
        //Skull Cleaver   0.3125
        //Solan	0.3125
        //Soldier	0.3125
        //Sophia	0.3125
        //Sorcerer	0.3125
        //Spine Hewer 0.3125
        //Spirit of the High Cleric	0
        //Spitting Bones  0.3125
        //Swift Skull Cleaver	0.3125
        //Tashun the Miner	0.3125
        //The Guardian    0.3125
        //Thenia	0.3125
        //Tilnan the Collector	0.3125
        //Tired Patron    0.3125
        //Transmogrify	0.3125
        //Traveling Scholar   0.3125
        //Travelling Merchant 0.3125
        //Treasure Hunter 0.3125
        //Tristram Militia    0.3125
        //Tristram Militia Recruit	0.3125
        //TristramGuard_C_NoWander	0.3125
        //Tyrael	0.3125
        //Urshi	0.3125
        //Vendel the Armorsmith	0.3125
        //Victor	0.3125
        //Victor the Quartermaster	0.3125
        //Vidar the Collector	0.3125
        //Voice from the Cellar   0.3125
        //Wardrobe	0.3125
        //Warrior Angel   0.3125
        //Warriv	0.3125
        //Adenah the Curio Vendor 0.3125
        //Alaric	0.3125
        //Andarian the Weaponsmith	0.3125
        //Westmarch Citizen   0.3125
        //Westmarch Guard 0.3125
        //Westmarch Peasant   0.3125
        //Westmarch Prisoner  0.3125
        //Westmarch Sailor    0.3125
        //Will	0.3125
        //Willa Rathe 0.3125
        //Andre	0.3125
        //Angel Trooper   0.3125
        //Angelic Host    0.3125
        //Angelic Scout   0.3125
        //Arghus the Collector	0.3125
        //Armored Risen Servitor	0.3125
        //Auriel	0.3125
        //Axe Bad Data	0.3125
        //Wortham Guard   0.3125
        //Wortham Militia 0.3125
        //Wounded Angel   0.3125
        //Wounded Guard   0.3125
        //Barnabas	0.3125
        //Bastion's Keep Sentry	0.3125
        //Bennoc	0.3125
        //Blazing Bone Axe Wielder    0.3125
        //Blinded Guard   0.3125
        //Bloated Corpse  0.3125
        //Blood Clan Occultist	0.3125
        //Blood Clan Sorcerer	0.3125
        //Botulph the Miner	0.3125
        //Briella	0.3125
        //Bron the Barkeep	0.3125
        //Young Guard 0.3125
        //Brother Andreus the Healer  0.3125
        //Brother Anselm  0.3125
        //Brother Francis 0.3125
        //Zaven the Alchemist	0.3125
        //Brother Ghaine the Healer   0.3125
        //Brother Malachi the Healer  0.3125
        //Brycen	0.3125
        //caldeumMiddleClass_Male_C_BreaksPots	0.3125
        //Captain	0.3125
        //Captain Cyrilis 0.3125
        //Captain Haile   0.3125
        //Captain Nowell  0.3125
        //Captain Rumford 0.3125
        //[TEMP]
        //Bastion's Keep Guard	0.3125
        //Captain Vonn    0.3125
        //Caravan Leader  0.3125
        //Eran	0.3125
        //Catapult Guard  0.3125
        //[TEMP]
        //PVP_Spawner	0.3125
        //[TEMP]
        //PVP_Tower	0.3125
        //Clara	0.3125
        //Colonel Severyn 0.3125
        //Covetous Shen   0.3125
        //Cow King's Minion	0.3125
        //Crazed Hermit   0.3125
        //Crazy Man   0
        //Cuthbert the Cobbler	0.3125
        //Damotrius	0.3125
        //Dark Evoker 0.3125
        //Dark Moon Clan Warrior  0.3125
        //{PH} Primeval Hunter    0
        //Deckard Cain    0.3125
        //Delilah the Collector	0.3125
        //Derric	0.3125
        //Servant of Jondar	0.3
        //Rebel Soldier   0.3
        //Dust Retcher    0.375
        //Painting	0.3
        //Corrupted Angel 0.25
        //Lost Marauder   0
        //Imprisoned Marauder Spirit	0.25
        //Jason Bender    0.25
        //Desert Raptor   0.25
        //James Southall  0.25
        //John Chambers   0.25
        //James Southall  0.25
        //Sungjae Cho 0.25
        //Lawrence Peacock    0.25
        //Mystic Ally 0.25
        //Michael Schwan  0.25
        //SummoningMachine	0.25
        //Michael Johnson 0.25
        //Shawn Su    0.25
        //Jake Sones  0.25
        //Michael Chu 0.25
        //John Heuerman   0.25
        //Nate Bowden 0.25
        //Spear Floor 0.25
        //Writhing Deceiver   0.25
        //Neal Kochhar    0.25
        //Neal Wojahn 0.25
        //Wretched Mother 0.25
        //Summoned Shield Guard	0.25
        //Dan Hernberg    0.25
        //Nephalem Ghost  0.25
        //Dennis Crabtree 0.25
        //Demonic Hellflyer   0.25
        //David Rovin 0.25
        //Shrieking Terror    0.25
        //Nick Eberle 0.25
        //Nick McVeity    0.25
        //Dark Hellion    0.25
        //Nick Slough 0.25
        //Nicole Gillett  0.25
        //Succubus	0.25
        //John Yang   0.25
        //Sibilant	0.25
        //Subjugator	0.25
        //Lashing Creeper 0.25
        //Infernal Bovine Shaman	0.25
        //Imprisoned Subjugator   0.25
        //Wizard_MirrorImage_Male	0.25
        //Wizard_MirrorImage_Female	0.25
        //Non Thareechit  0.25
        //Keith Mitchell  0.25
        //Jonathan Hwang  0.25
        //Larra Paolilli  0.25
        //Jonathan Mankin 0.25
        //Julian Love 0.25
        //Imprisoned Assaulter Spirit	0.25
        //Wintersbane Huntress    0.25
        //Kenneth Williams    0.25
        //Oliver Chipping 0.25
        //Winged Molok    0.25
        //Ophelia	0.25
        //Winged Assassin 0.2125
        //Winged Assasin  0.2125
        //Siegebreaker Assault Beast	0.25
        //Matthew Panepinto   0.25
        //Oscar Carlen    0.25
        //Wickerman	0.25
        //Ice Porcupine   0
        //Wicker Man  0.25
        //Ice Clan Shaman	0.25
        //Skull Summoner  0.3
        //Stone Guardian  0.25
        //Hungry Corpse   0.25
        //Matt Sherman    0.25
        //Patriarch Anisim    0.25
        //Patrick Dawson  0.25
        //Patrick Stone   0.25
        //Matt Ryan   0.25
        //Paul David  0.25
        //Paul Menichini  0.25
        //Daniel Briggs   0.25
        //Matt R.Taylor  0.25
        //Webspitter Spider	0.25
        //Matt Panepinto	0.25
        //Peet Cooper	0.25
        //Steve Parker	0.25
        //Horrific Mimic	0.25
        //Deathspitter    0.25
        //Warscarred Footsoldier	0.25
        //High HP Test Dummy	0.25
        //Stephen Wong	0.25
        //Soldier of Emikdeva 0.25
        //Spotting Tower	0.25
        //Accursed Hellion	0.25
        //Philippe Colin	0.25
        //Phillip Dettorre	0.25
        //SporePod    0.25
        //Hell Witch	0.25
        //Hell Fissure	0.25
        //Lethal Decoy	0.25
        //Helder Pinto	0.25
        //Ken DePalo	0.25
        //Spirit Ring	0.25
        //Plague Swarm	0.25
        //Adam York	0.25
        //Ancient Bomber	0.25
        //Andrea Toyias	0.25
        //Dark Beast	0.25
        //Death Swarm	0.25
        //Jonny Lien	0.25
        //Vitaliy Naymushin	0.25
        //Andrew Vestal	0.25
        //Anessa Silzer	0.25
        //Spike Trap	0.25
        //Level 61 Target Dummy	0.25
        //Vile Temptress	0.25
        //Vile Swarm	0.25
        //Prison Guard	0.25
        //Angela Shih	0.25
        //Prison Warden	0.1875
        //Spellwinder 0.25
        //Lost Shaman	0.25
        //Mike Hershberg	0.25
        //Mike Nicholson	0.25
        //Gore Harrier	0.25
        //Johannes The	0.25
        //Joseph Floro	0.25
        //Putrid Hive	0.25
        //Zombie Dog	0.25
        //Zombie Prisoner	0.25
        //Glenn Stafford	0.25
        //Vicious Hellion	0.25
        //Joel Taubel	0.25
        //Vic Lee	0.25
        //Quang Tran	0.25
        //Anthony Rivero	0.25
        //Lacuni Stalker	0.25
        //Ghostly Murderer	0.25
        //Arcane Sentry	0.25
        //Vengeful Summoner	0.3
        //Armored Destroyer	0.25
        //Jesse McCree	0.25
        //Ghostly Attendant	0.25
        //Rachel Larsen	0.25
        //Ghost of the Cow King   0.25
        //Art Peshkov	0.25
        //Ghost of Captain Stokely	0.25
        //Asher Litwin	0.25
        //Mongsub Song	0.25
        //Monk_LethalDecoy_RuneC  0.25
        //[TEMP] Mirror Image	0.25
        //Level 10 Target Dummy	0.25
        //Vault Sentry	0.25
        //Keith Landes	0.25
        //Vault Peon	0.25
        //Azmodan Lazer Attack Proxy	0.25
        //Banshee 0.25
        //Moon Clan Shaman    0
        //[TEMP] Raven    0.25
        //Joseph Lawrence	0.25
        //Joni Cheng	0.25
        //Lilith  0.25
        //[TEMP] Split Monster	0.25
        //[TEMP] Triune Summon	0.25
        //[TEMP] Wizard Meteor Pulsar 0.25
        //Twinkleroot 0.25
        //{ PH}
        //Abyssal Caller 0
        //Frenzied Hellion    0.25
        //{PH} Abyssal Protector  0
        //Foster Elmendorf    0.25
        //Jeff Kang   0.25
        //Jeff Hyeongwon Kang	0.25
        //Morlu Centurion 0.25
        //Rebel Archer    0.25
        //Tristram Chapel 0.25
        //Bobby Koh   0.25
        //Jay Maguire 0.25
        //Trent Kaniuga   0.25
        //Bree Powers 0.25
        //Morlu Invader   0.25
        //Morlu Legionnaire   0.25
        //Luis Barriga    0.25
        //{ph} Primal Splinter    0
        //Travis Day  0.25
        //Trapped Marauder    0.25
        //{PH} Vengeful Remains   0
        //Tortured Summoner   0.3
        //Jason Briggs    0.25
        //Dave Pendergrast    0.25
        //Brigand	0.25
        //Returned Summoner   0.3
        //Brigand Looter  0.25
        //Brigham	0.25
        //Tomb Guardian   0.3
        //Tom Ryan    0.25
        //{PH} Wood Specter   0
        //Richie Marella  0.25
        //Timothy Linn    0.25
        //Timothy Ismay   0.25
        //Fallen Shaman   0.25
        //Brood Daughter  0.25
        //Kritter Griffin 0.25
        //Risen Servitor  0.25
        //Darksky Fire Demon	0.25
        //Rob Martin  0.25
        //Robert	0.25
        //Bryan Gidge 0.25
        //Fallen Prophet  0.25
        //Fallen Mutt 0.25
        //Tim Linn    0.25
        //Fallen Magus    0.25
        //Julien Lefebvre 0.25
        //Roger Hughston  0.25
        //Tiffany Wat 0.25
        //Ron Gray    0.25
        //Bryan Marony    0.25
        //Caldeum Commoner    0.25
        //Caller of the Damned    0.25
        //Ruins Firewing  0
        //Russ Foushee    0.25
        //Russell Brower  0.25
        //Slime	0.25
        //Thieves Guild Robber	0.25
        //Thieves Guild Brigand	0.25
        //Sébastien Poirier   0.25
        //Fallen Firemage 0.25
        //Thieves Guild Bandit	0.25
        //Slime Spewer    0.25
        //Fallen Conjurer 0.25
        //Executioner's Blade	0.25
        //Exarch	0.1875
        //Darkmoth	0.25
        //Evan Chen   0.25
        //Eric Spevacek   0.25
        //Chris Amaral    0.25
        //	0.25
        //Entangling Roots    0.25
        //Enslaved Nightmare  0.25
        //The Dancing Shaman	0.25
        //Chris Haga  0.25
        //Chris Jacobson  0.25
        //Clay Murphy 0.25
        //Enraged Spirit  0.25
        //Scaled Magus    0.25
        //Clayton Vaught  0.25
        //Clint Walls 0.25
        //Soul Ripper 0.25
        //Colm Nelson 0.25
        //Copperfang Lurker   0.25
        //Elaine Yang 0.25
        //Edan Bryant 0.25
        //Scott Gordon    0.25
        //Scott Keenan    0.25
        //Scott Lawlor    0.25
        //Soul Lasher 0.25
        //Templar Arbalest    0.25
        //Sebastien Poirier   0.25
        //Kevin K.Griffith   0.25
        //Kevin K Griffith    0.25
        //Corruption Geyser	0.25
        //Soul Crucible	0.25
        //Serpent Magus	0.25
        //Kevin Carter	0.25
        //Doom Viper	0.25
        //Swinging Crescent	0.25
        //Dohyeong Kim	0.25
        //Swift Fleshmauler	0.25
        //Swift Flayer Demon  0.25
        //Cursed Shaman	0.25
        //Retching Cadaver	0.25
        //Spewing Horror	0.25
        //Vicious Hound	0.2125
        //Revenant Soldier	0.2125
        //PVP_stationary_defender 0.2
        //Cuddle Bear	0.25
        //[TEMP] PVP_Stationary_Defender  0.2
        //Jacob Rodriguez	0.1875
        //Lawrence King	0.1875
        //Nicholas Brothers	0.1875
        //Wraith  0.1875
        //Meriel Regodon	0.1875
        //Jon Briggs	0.1875
        //Long Pham	0.1875
        //Winged Talus	0.1875
        //Matthew Rader	0.1875
        //Matt Worcester	0.1875
        //James Sutton	0.1875
        //Hulking Destroyer	0.1875
        //Matt Grenewetzki	0.1875
        //Henry Kung	0.1875
        //Matt Fieler	0.1875
        //Phillip Williams	0.1875
        //Adam Baxter	0.1875
        //Harpagon    0.1875
        //Grim Wraith	0.1875
        //Ponzi   0.1875
        //Graig Taylor	0.1875
        //Gorgon Gekko	0.1875
        //Vile Revenant	0.1875
        //Victoria Tao	0.1875
        //Goldenfingers   0.1875
        //Soulless Feeder	0.1875
        //Lewis Villamar	0.1875
        //Vengeful Phantasm	0.1875
        //George Shute	0
        //Garrett Elmendorf	0.1875
        //Garret Craig	0.1875
        //Baron Slothschild	0.1875
        //Bear Boesky	0.1875
        //Blazing Bone Warrior    0.1875
        //Joseph Romano	0.1875
        //Tyloss Lannister	0.1875
        //Ravenous Feeder	0.1875
        //Josh Greenfield	0.1875
        //Foul Conjurer	0.1875
        //Foster Elmendorf	0.1875
        //Joshua Ellington	0.1875
        //Rebecca Lin	0.1875
        //Trevor Page	0.1875
        //Brent Brewington	0.1875
        //Flesh Hurler	0.1875
        //Brian Boswell	0.1875
        //Brian Urbina	0.1875
        //Dustin Trimble	0.1875
        //Demon Trooper	0.1875
        //Sean Masterson	0.1875
        //Teak Holley	0
        //Demon Raider	0.1875
        //Chase Hall	0.1875
        //Demon Marauder	0.25
        //Rezenebe Scruuge	0.1875
        //Steven Worcester	0.1875
        //Mydas   0.1875
        //Dark Summoner	0.1875
        //Steve Martinez	0.1875
        //Kevin Langendoen	0.1875
        //Ryan Felton	0.1875
        //Deathly Haunt	0.1875
        //Daniel Helwig	0.1875
        //Daniel Polcari	0.1875
        //Mounty Burns	0.1875
        //Darian Vorlick	0.1875
        //Savage Ghoul	0.1875
        //Ryan Matsuba	0.1875
        //Enraged Phantom	0.1875
        //Enraged Phantasm	0.1875
        //Crazed Summoner	0.1875
        //Dark Conjurer	0.1875
        //Kayleigh Calder	0.1875
        //Tony Misgen	0.1875
        //Michael Sassone	0.1875
        //Jakeb Howard	0.1875
        //Duncan Field	0.1875
        //Rolph Duke	0.1875
        //Kroisos 0.1875
        //Clint Walls	0.1875
        //Sewer Serpent	0.1875
        //Sewer Worms	0.1875
        //Erik Larson	0.1875
        //Joe Truong	0.1875
        //Jimmie Jaimes	0.1875
        //Swift Fleshripper	0.1875
        //Ron Hodge	0.1875
        //Shadow Guard	0.1875
        //Neil Shapiro	0
        //Shadow of Death 0.1875
        //Shadow Swordwielder	0.1875
        //Kevin Zhao	0.1875
        //Chris Davila	0.1875
        //Eric Covington	0
        //Derek Rakos	0.1875
        //Kea Yonni	0.1875
        //Mad Hoff	0.1875
        //Cathy Lee	0.1875
        //Cynthia Hall	0.1875
        //Robert Pionke	0.1875
        //Tim Toledo	0.1875
        //Jay Briggs	0.1875
        //Scott Trujillo	0.1875
        //Denis Genest	0.1875
        //Justen Quirante	0.1875
        //Mort Duke	0.1875
        //{ PH}
        //Shipwrecked Soul   0
        //Jason K Kwon	0.1875
        //{ph} Uncontained Spirit 0
        //Cornelius Panderbilt    0.1875
        //TEMP Core Elite Demon   0.1875
        //Keith Kodama    0.1875
        //Chilling Construct  0.165
        //Smoldering Construct    0.165
        //Anne-Sophie Lefebvre    0.15
        //Voracious Zombie    0.25
        //Alex Mayberry   0.15
        //Matt Burns  0.15
        //Matthew Ryan    0.15
        //Jon Dvorak  0.15
        //Wyatt Cheng 0.15
        //John Heuerman   0.15
        //Michael Hershberg   0.15
        //Nathan Bowden   0.15
        //Nicholas Eberle 0.15
        //Nick Rivera 0.15
        //Johannes Thé    0.15
        //Norbert Szabo   0.15
        //Ice Clan Impaler	0.1
        //Paul Warzecha   0.15
        //Henry Ho    0.15
        //Randal Dumoret  0.15
        //Jill Harrington 0.15
        //Kyle Jensen 0.15
        //Rob Foote   0.15
        //Russell Foushee 0.15
        //Kevin Martens   0.15
        //Jeremy Craig    0.15
        //Evan Greenstone 0.15
        //Elizabeth Cho   0.15
        //Scott Mercer    0.15
        //Dust Shambler   0.1875
        //Kevin Griffith  0.15
        //Derek Duke  0.15
        //Decayer	0.25
        //Ken Lamb    0.15
        //Skye Chandler   0.15
        //David M.Adams  0.15
        //Pior Oberson	0.15
        //Steve Shimizu	0.15
        //Corey Peagler	0.15
        //Chris Ryder	0.15
        //Chris De La Pena	0.15
        //Justin Dye	0.15
        //Carl Lavoie	0.15
        //Jason Regier	0.15
        //Tina Fu	0.15
        //Brian Fletcher	0.15
        //Juan Custer	0.15
        //Josh Tallman	0.15
        //Valerie Watrous	0.15
        //Arthur Flew	0.15
        //Leonard Boyarsky	0.15
        //{ PH}
        //Amputator	0
        //Returned Shieldman  0.125
        //Ghostly Mason   0.125
        //Humbart Wessel  0.125
        //Noxious Guardian    0.125
        //Stinging Swarm  0.125
        //Revenant Archer 0.125
        //Ghastly Gravedigger 0.125
        //Blazing Swordwielder    0.125
        //Gravedigger	0.125
        //Ravenous Dead   0.125
        //DebugPlainDog	0.125
        //Ferryman	0.125
        //Skeletal Marauder   0.125
        //Skeletal Raider 0.125
        //Skeletal Sentry 0.125
        //Skeletal Shieldbearer   0.125
        //Cursed Tristram Archer	0.125
        //Ravager	0.125
        //graveDiggerCrownScript	0.125
        //Acid Hydra  0.125
        //Burrowing Leaper    0.125
        //Jailed Deserter 0.125
        //Inferno Zombie  0.125
        //Forgotten Shield Bearer	0.125
        //Skeletal Crawler    0.125
        //Frostbiters	0.125
        //Eric Maloof 0.1125
        //Don Vu  0.1125
        //Larra Paolilli  0.1125
        //Orry Suen   0.1125
        //Andrew Chambers 0.1125
        //Matt McDaid 0.1125
        //Cory Robinson   0.1125
        //Kevin Hassett   0.1125
        //Joshua Manning  0.1125
        //Matthew Berger  0.1125
        //Sean Riley  0.1125
        //Lorenzo Minaca  0.1125
        //Victor Lee  0.1125
        //Pat Nagle   0.1125
        //Arnold Tsang    0.1125
        //Jay Patel   0.1125
        //David Pendergrast   0.1125
        //Stuart Capewell 0.1125
        //Ken Morse   0.1125
        //Christian Lichtner  0.1125
        //Aaron Gaines    0.1125
        //Chris Donelson  0.1125
        //Sojin Hwang 0.1125
        //Jonny Ebbert    0.1125
        //Ben Zhang   0.1125
        //Jeremy Masker   0.1125
        //Joe Shely   0.1125
        //Quang D.Tran   0.1125
        //Katie Bryant	0.1125
        //Chris Allsopp	0.1125
        //Zaven Haroutunian	0.1125
        //Ryan Pearson	0.1125
        //John Buckley	0.1125
        //Michael Nicholson	0.1125
        //James Peterson	0.1125
        //Tiffany K. Wat  0.1125
        //Kris Giampa	0.1125
        //Nathan Lutsock	0.1125
        //Nick Chilano	0.1125
        //John Hight	0.1125
        //Robert T. Martin Jr.	0.1125
        //Peter Lee	0.1125
        //Nigel Nikitovich	0.1125
        //Grace Liu	0.1125
        //Richard Chu	0.1125
        //Tom Gerber	0.1125
        //Matthew Sherman	0.1125
        //Brian Kindregan	0.1125
        //Julia Humphreys	0.1125
        //Bone Warrior	0.125
        //Quill Demon	0.1
        //Frost Hydra	0.1
        //Blazing Guardian	0.1
        //Cursed Tristram Warrior 0.1
        //Summoned Soldier	0.1
        //Dark Skeletal Bowman    0.1
        //Submissive Prisoner	0.1
        //Ghoul   0.1
        //Skeletal Archer	0.1
        //Infernal Bovine Spearman    0.1
        //Blazing Bone Archer 0.1
        //Leah    0.1
        //Moon Clan Impaler   0.1
        //Dark Skeletal Archer    0.1
        //[TEMP] Rockfall 0.1
        //Returned Archer	0.1
        //Carrion Bat	0.1
        //Returned    0.125
        //Imperius    0.1
        //Dark Wizard	0.1
        //Thieves Guild Assassin  0.1
        //Fire Hydra	0.1
        //Thieves Guild Fighter   0.1
        //Icy Quillback	0.1
        //Dark Moon Clan Impaler	0.1
        //Flaming Nephalem Spirit 0.1
        //Collapsing Ceiling	0.1
        //Jailed Poacher	0.1
        //Frost Razor	0.1
        //Quill Fiend	0.1
        //Skeletal Ranger	0.1
        //Skull Sword	0.125
        //Minion of Death 0.125
        //Skeletal Bowmaster	0.1
        //Skeletal Warrior	0.125
        //Ghostly Khazra Spearman 0.1
        //Disturbing Mosquito Bat 0.075
        //{ ph}
        //Hunter Bat 0
        //Cult Leader 0.075
        //Dune Stinger    0.05
        //{ph} Lamprey	0
        //Shock Guardian  0.075
        //Crazed Cultist  0.075
        //Deranged Cultist    0.075
        //Desert Hornet   0.05
        //Kala	0.075
        //Corrupt Vessel  0.075
        //Corpse Worm 0.075
        //Emperor Hakan II	0.075
        //Citizen of the Keep 0.075
        //Enraged Zealot  0.075
        //Sasha	0.075
        //Angry Farmer    0.075
        //Dark Vessel 0.075
        //Chiman	0.075
        //Sand Wasp   0.05
        //Child	0.075
        //Chanting Cultist    0.075
        //Cave Bat    0.075
        //Bunny	0.075
        //Risen	0.075
        //Rike the Apothecary	0.075
        //Dark Ritualist  0.075
        //Kyla	0.075
        //Reprobate Vessel    0.075
        //Refugee Leader  0.075
        //Reformed Cultist    0.075
        //Former Noblewoman   0.075
        //Reaper	0.075
        //Small Boy   0.075
        //Blood Clan Spearman	0.075
        //Blood Clan Impaler	0.075
        //Blood Child 0.075
        //Betrayed	0.05
        //Dark Zealot 0.075
        //Frostclaw Burrower  0.075
        //Fuad	0.075
        //Rasheed	0.075
        //Veiled Evoker   0.075
        //Lacuni Huntress 0.075
        //Dark Cultist    0.075
        //Victim	0.075
        //Marta	0.075
        //Glowing Death   0.05
        //Master Soul Crucile	0.075
        //Vile Bat    0.075
        //Poltahr	0.075
        //Alcarnus Refugee    0.075
        //Guardian Tower  0.075
        //Accursed	0.05
        //Abheek	0.075
        //Dark Greater Vessel	0.075
        //Perfidious Cultists 0.075
        //Hooded Nightmare    0.075
        //Little Girl 0.075
        //Impoverished Refugee    0.075
        //Urchin	0.075
        //Noblewoman	0.075
        //Nobleman	0.075
        //Lightning Hydra 0.075
        //Leaper	0.075
        //Mia	0.075
        //Little Boy  0.075
        //Minion of Azmodan	0.075
        //Kayla	0.075
        //Squirt the Peddler	0.075
        //Dark Soul   0.075
        //Dane Bright 0.075
        //SkeletonKing_Target_Proxy	0.0625
        //Death Bomber    0.0625
        //Rotting Imp 0.0625
        //Plague Carrier  0.0625
        //Dust Imp    0.0625
        //Vile Familiar   0.0625
        //Pack Hunter 0
        //Vile Hellbat    0.0625
        //Giant Shadow Wing	0.0625
        //Demon Flyer 0.0625
        //Skeleton Minion 0.0625
        //Inferno Carrion 0.0625
        //Stygian Crawler 0.0625
        //Summoned Archer 0.0625
        //Atramental Creation 0.0625
        //Desiccated Imp  0.0625
        //Cave Wing   0.0625
        //Tormented Stinger   0.0625
        //Royal Henchman  0.0625
        //Charged Stinger 0.0625
        //Savage Flyer    0.0625
        //Necromantic Minion  0.0625
        //Scavenger	0.05
        //Frost Maggot    0.05
        //{ph} Empty Husk 0
        //Reviled	0.05
        //Toxic Construct 0.05
        //Blazing Ghoul   0.05
        //Frost Guardian  0.05
        //Vicious Ghoul   0.05
        //Charged Construct   0.05
        //Hungerer	0.05
        //Arcane Hydra    0.05
        //Angelic Warrior 0.05
        //Murderous Fiend 0.05
        //Savage Fiend    0.05
        //Furnace Vent    0.05
        //Fallen Peon 0.0375
        //Fallen Maniac   0.0375
        //Fallen Lunatic  0.0375
        //Fallen Grunt    0.0375
        //Demented Fallen 0.0375
        //Fallen Devotee  0.0375
        //Fallen	0.0375
        //Maggot	0.0375
        //Skeleton	0.0375
        //Fallen Soldier  0.0375
        //Scouring Charger    0.0375
        //{ph} Oozing Arachnid    0
        //Frenzied Lunatic    0.0375
        //Scouring Lobber 0.0375
        //Deranged Fallen 0.0375
        //Mad Nagelspawn  0.0375
        //Fallen Fighter  0.0375
        //Cursed Pilgrim  0.0375
        //TEMP Ordnance   0.025
        //Mine	0.025
        //Demon Gate  0.025
        //Timeless Prison 0.025
        //Naja Beetle 0.0125
        //Shock Tower 0.025
        //Cailyn	0.0125
        //caldeumTortured_Poor_Male_A_RitualVictim	0.0125
        //Young Quill Fiend	0.0125
        //Apparition	0.0125
        //Zap Worm    0.0125
        //Voracious Torso 0.0125
        //Dust Biter  0.0125
        //Crazed Imp  0.0125
        //Soulless Corpse 0.0125
        //Cave Shrew  0.0125
        //Zombie Struggler    0.0125
        //Crawling Torso  0.0125
        //Cavern Spider   0.0125
        //Spine Lasher    0.0125
        //The Stranger    0.0125
        //Bile Crawler    0.0125
        //Plagued Vermin  0.0125
        //Explosive Vermin    0.0125
        //Shade Stalker   0.0125
        //Sandling	0.0125
        //Ratling	0.0125
        //Son of Nichol	0.0125
        //Private Mattius 0.0125
        //Scarab	0.0125
        //Prison Scavenger    0.0125
        //Grace	0.0125
        //Peasant	0.0125
        //Terror Spawn    0.0125
        //Lady Gwyndolin  0.0125
        //Emily	0.0125
        //Gloom Wraith    0.0125
        //Cob Marin   0.0125
        //Refugee	0.0125
        //Hungry Torso    0.0125
        //Boggit	0.0125
        //Electric Eel    0.0125
        //Shrivel	0.0125
        //Westmarch Villager  0.0125
        //Scoundrel	0.0125
        //Former Nobleman 0.0125
        //Lord Stuart 0.0125
        //Skeleton_Cain	0.0125
        //Imp	0.0125
        //Acid Slime  0.0125
        //Soul Essence of Azmodan 0.0125
        //Grave Spirit    0.0125
        //Villager	0.0125
        //Imprisoned Spectre  0.0125
        //Fiend	0.0125
        //Lady Ansonia    0.0125
        //Virulent Ratling    0
        //Liria	0.0125
        //Shadow Vermin   0.0125
        //Survivor	0.0125
        //Brood Hatchling 0.0125
        //Swift Fleshcrawler  0.0125
        //Spiderling	0.0125
        //Sewer Scuttler  0.0125
        //[ph]
        //Breath Yeti    0.875
        //Mange	3.125
        //Punchout	3.125
        //[ph]
        //Death Construct    0.375
        //[ph]
        //Beam Yeti  0.375
        //[ph]
        //Infection Construct    0.5
        //[ph]
        //Knock Sasquatch    0.875
        //Kankerrot	3.125
        //[ph]
        //Wave Sasquatch 0.75
        //[ph]
        //Block Sasquatch    0.75

    }
}
