# Dino — návrh levelov

Pracovný dokument s témami levelov. Otázniky sú veci, ktoré ešte nie sú rozhodnuté.

## Prehľad

| Level | Názov | Ročné obdobie | Téma | Čo padá | Boss |
|---|---|---|---|---|---|
| 1 | Meteor Madness | Jeseň | Praveká / prírodná | Meteority | Meteor Boss |
| 2 | Snowfall Survival | Zima | Doba ľadová | ? | ? |
| 3 | Modern Mayhem | Jar | Moderná doba | Telefóny, autá? | Žeriav s demolačnou guľou? |
| 4 | Renewal Rift | Leto | Budúcnosť / sci-fi | ? | Mimozemšťania (UFO) |
| 5 | Bubble (DLC) | ? | Bubble tea | Bubbles z lietajúcich bubble tea | Kráľovná ôs? |
| 6 | ? | ? | DLC | ? | ? |

## Stav rozhodnutí

**Odsúhlasené**
- Level 2: roztápajúci sa ľad ako tretia plošina, pozadie ľadovej jaskyne
- Level 3: **Pizza Delivery** ako špeciálna udalosť (dron zhadzuje pizzu na hlavu) — dron teda **nie je** boss
- Level 4: trojica plošín (hologram, antigravitácia, silové pole)
- Levely 5 a 6 sú DLC
- Level 5: bubble tea, lepkavé škvrny, osa ako trest za státie na mieste

**Čaká na rozhodnutie** — viď sekciu *Otvorené otázky* nižšie

---

## Level 1 — Meteor Madness (jeseň)

**Téma:** praveká, prírodná

**Čo padá:** meteority

**Prostredie:**
- plošinky
- oblaky
- kamene

**Boss:** Meteor Boss — hotový

**Stav:** implementovaný (scéna `Dino`)

---

## Level 2 — Snowfall Survival (zima)

**Téma:** doba ľadová (ice age)

**Čo padá:** ? — nerozhodnuté

**Prostredie:**
- plošinky
- šmykľavé plošinky (ľad)
- ? — čo ďalšie

**Boss:** ? — nerozhodnuté

---

## Level 3 — Modern Mayhem (jar)

**Téma:** moderná doba

**Čo padá:** telefóny? autá? — nerozhodnuté

**Prostredie:**
- plošinky
- ? — čo ďalšie

**Boss:** ? — nerozhodnuté

---

## Level 4 — Renewal Rift (leto)

**Téma:** budúcnosť, futuristická doba

**Čo padá:** ? — nerozhodnuté

**Prostredie:**
- ? — nerozhodnuté

**Boss:** mimozemšťania

**Poznámka k bossovi:** UFO po hráčovi strieľajú (na rozdiel od Meteor Bossa, ktorý hádže mini meteority)

---

## Levely 5 a 6 — DLC

Základná hra má 4 levely, levely 5 a 6 sú plánované ako DLC.

V kóde už s nimi počíta `LevelUnlocker.totalLevels = 6` aj `MainMenu.knownLevels` (6 položiek), takže po strane odomykania a štatistík je miesto pre ne pripravené.

### Level 5 — Bubble (DLC)

**Téma:** bubble tea

**Plošiny** *(návrh)*
- **Bublina** — priehľadná plošina, ktorá po chvíli státia praskne. To isté ako praskajúca kryha v leveli 2, len inak oblečené.
- **Slamka** — dlhá šikmá plošina, po ktorej hráč pomaly skĺzava dole. Dá sa po nej vyjsť, ale nie na nej stáť.
- **Kopa tapioky** — mäkký povrch, hráč sa po ňom pohybuje pomaly (rovnaká logika ako sticky, len natrvalo).
- **Viečko pohára** — obyčajná pevná plošina, základ levelu.

**Čo lieta a padá**
- **Bubble tea** — poháre lietajú ponad hráča (nepadajú zvisle ako meteority, pohybujú sa hore nad úrovňou hrania)
- **Bubbles** — poháre po hráčovi pľujú/strieľajú guličky (tapioka)
- **Kocky ľadu** *(návrh)* — vypadnú z pohárov a po dopade spravia **šmykľavú** plochu. Spolu so sticky tak máš v jednom leveli dva opačné povrchy — jeden hráča spomaľuje, druhý mu berie kontrolu.
- **Slamky** *(návrh)* — úzke a rýchle, padajú takmer zvisle. Obdoba cencúľov.

**Sticky mechanika**
- Keď bubble dopadne na zem, ostane po nej **lepľavá škvrna**
- Po lepkavej ploche sa **ťažko chodí** — hráča spomaľuje
- Škvrny zostávajú, takže sa postupne kopia a level sa sám robí ťažším

**Osa / včela — trest za kempenie**
- Ak hráč zostane **pridlho na jednom mieste**, kde je vidieť lepkavú škvrnu, priletí **osa alebo včela** a sadne si na ňu
- Osa hráčovi **ubere život**
- Zmysel: hráč nesmie stáť na mieste a čakať, musí sa stále presúvať

**Boss — Kráľovná ôs** *(návrh)*

Celý level hráča učí jednu vec: *lepkavé škvrny priťahujú osy, takže sa nesmieš zdržiavať*. Boss by mal presne toto obrátiť proti hráčovi — inak je to len ďalší nepriateľ, ktorý strieľa.

- **Fáza 1 — zaplavenie.** Kráľovná sa vznáša hore a pľuje veľké bubliny, ktoré po dopade robia lepkavé škvrny. Hráčovi sa postupne zmenšuje plocha, kde sa dá normálne hýbať.
- **Fáza 2 — roj.** Privoláva robotnice, ktoré si sadajú na škvrny a útočia na hráča, ak sa priblíži. Hráč ich musí strieľať, ale tým míňa náboje, ktoré potrebuje na kráľovnú.
- **Fáza 3 — kŕmenie.** Keď klesne na tretinu života, sama si sadne na najväčšiu lepkavú škvrnu, aby sa nakŕmila. **Iba vtedy je zraniteľná.**

Z toho vzniká pekné napätie: hráč potrebuje, aby na mape **bola** veľká lepkavá škvrna (inak si kráľovná nesadne a nedá sa zabiť), ale zároveň mu tá škvrna prekáža v pohybe a priťahuje osy. Musí si teda vedome nechať jedno miesto zaprataté a ostatné si udržiavať čisté.

**Slabina:** krídla — kým lieta, dá sa jej ubrať len minimum; keď sedí, ide to naplno.

*Alternatíva, ak by bola kráľovná priveľa práce:* **obrí pohár bubble tea**, ktorý slamkou nasáva vzduch a ťahá hráča k sebe, a striedavo vypľúva dávky tapioky. Jednoduchšie na animáciu aj na kód, ale nevyužije to osy.

**Poznámky k implementácii**
- Lepkavá škvrna je opak ľadu z levelu 2: tam nulové trenie a kĺzanie, tu spomalenie. Technicky stačí znížiť `moveSpeed` hráča, kým stojí v škvrne.
- Lietajúce bubble tea sa správa inak než `TargetSpawner` (ten spúšťa objekty zvisle zhora) — potreboval by vlastný pohyb ponad hráčom a streľbu nadol, podobne ako materská loď v leveli 4.
- Osa je časovač naviazaný na to, ako dlho je hráč blízko tej istej škvrny.
- Kocky ľadu a lepkavé škvrny sú ten istý skript s opačným parametrom trenia — netreba na ne dve riešenia.
- Kráľovná ôs sa dá postaviť na `MeteorBossController`: vznáša sa, má fázy a spúšťa objekty. Nové je len sadanie na škvrnu a okno zraniteľnosti, čo je v podstate obdoba `SetAttackState()`, ktoré tam už je.

**Návrh názvu (nerozhodnuté):** ostatné levely majú dvojslovný aliteračný názov (Meteor Madness, Snowfall Survival, Modern Mayhem), takže by sedelo napr. *Bubble Blitz* alebo *Bubble Brew*.

### Level 6 — DLC

Téma zatiaľ nerozhodnutá.

---

## Otvorené otázky

- Level 2: čo padá a kto je boss
- Level 3: čo padá okrem telefónov/áut, kto je boss
- Level 4: čo padá, aké prostredie
- Level 5 (Bubble): kto je boss, presný názov levelu
- Level 6 (DLC): celá téma
- Majú mať levely inú mechaniku, alebo len iný vzhľad a iné padajúce objekty?

---

## Návrhy — plošiny, padajúce objekty, bossovia

Toto **nie sú rozhodnutia**, len konkrétny návrh od čoho sa odraziť. Je stavaný tak, aby väčšina vecí bola obmenou toho, čo už v hre funguje.

### Čo už máš a dá sa recyklovať

| Existujúce | Skript | Dá sa z toho spraviť |
|---|---|---|
| Pohyblivý oblak | `MovingCloud` | výťah (zvislý pohyb), bežiaci pás, plávajúca kryha |
| Kolísavý kameň | `StonePlatform` | každá plošina, ktorá sa pod hráčom nakláňa |
| Padajúce meteority | `TargetSpawner` + `Target` | akýkoľvek padajúci objekt, stačí iný sprite a rýchlosť |
| Meteor Boss | `MeteorBossController` | každý boss, čo sa vznáša a strieľa po hráčovi |

---

### Level 2 — Snowfall Survival (zima)

**Plošiny** — odporúčaná trojica

1. **Zasnežený kameň** — obyčajná pevná plošina. Musí tam byť, inak nie je kam sa stiahnuť a level je len frustrujúci.
2. **Ľadová** — šmykľavá, hráč po dopade kĺže a nezastaví okamžite. V kóde je to materiál s nulovým trením (hráč ho už má kvôli oblakom) plus zotrvačnosť.
3. **Roztápajúci sa ľad** — hneď ako naň hráč stúpi, začne sa topiť a asi po 1,5 s zmizne. Po pár sekundách znova zamrzne a dá sa použiť. **Toto je srdce levelu.**

Prečo práve táto trojica: každá plošina hovorí hráčovi niečo iné — *tu si v bezpečí*, *tu neovládaš pohyb*, *tu nesmieš zostať*. A kombinácia dvoch a troch je to, čo level robí ťažkým: roztápajúci sa ľad ťa núti rýchlo preč, ale šmykľavosť ti presne v tej chvíli berie kontrolu nad odrazom.

Dôležité pri ladení: nech je vidno, **kedy sa ľad začal topiť** (praskliny, kvapkanie, meniaca sa farba) a nech je aj počuť. Bez jasného signálu to hráč vníma ako nefér.

*Voliteľne navyše:* **plávajúca kryha** — `MovingCloud` s iným spritom, pomalšia a s väčšou dráhou. Hodí sa skôr ako oddychový úsek medzi náročnými pasážami.

**Čo padá**
- **Cencúle** — úzke, rýchle, padajú takmer zvisle. Najnebezpečnejšie, ale ľahko sa trafia.
- **Snehové gule** — veľké, pomalé, ale cestou naberajú rýchlosť.
- **Kusy ľadu** — po zásahu sa rozpadnú na dva menšie, ktoré treba dostreliť. Pekne to nadviaže na meteority z levelu 1, lebo je to ten istý princíp, len o stupeň ťažší.

**Boss — Ľadový golem** *(odporúčam)*

- Vznáša sa ako Meteor Boss, takže sa dá postaviť na `MeteorBossController`.
- **Fáza 1:** hádže po hráčovi cencúle vo vejári (obdoba mini meteoritov).
- **Fáza 2:** pri polovici života **zamrzne celú podlahu** — všetko sa stane šmykľavým, aj plošiny, ktoré boli predtým bezpečné.
- **Fáza 3:** pri tretine života nechá zamrznuté plošiny **roztápať sa rýchlejšie**, takže arénu postupne stráca.
- **Slabina:** po každej dávke na chvíľu odkryje ľadové jadro — len vtedy mu ide ubrať život.

Zmysel je rovnaký ako pri kráľovnej ôs v leveli 5: boss berie mechaniku, ktorú sa hráč celý level učil, a otočí ju proti nemu. Najprv si zvykol, že niektoré plošiny sú bezpečné a niektoré nie — a golem mu to rozdelenie zmaže.

*Alternatíva:* **mamut**, ktorý sa nevznáša, ale dupe po zemi a otrasmi zhadzuje cencúle zo stropu. Iný pocit z boja (pozemný, nie vzdušný), ale treba naň nový skript — golem sa dá poskladať z toho, čo už máš.

---

### Level 3 — Modern Mayhem (jar, moderná doba)

**Plošiny**
- **Lešenie** — obyčajná statická plošina, základ levelu.
- **Výťah** — `MovingCloud` otočený na zvislý pohyb. Hráč sa na ňom vezie hore a dole (mechanika už funguje).
- **Bežiaci pás** — plošina, ktorá hráča stále posúva do strany. Technicky je to to isté ako oblak, ktorý sa sám nehýbe, ale hráčovi pripočítava posun.

**Čo padá**
- **Telefóny** — malé a rýchle, ťažko sa trafia.
- **Autá** — veľké, pomalé, zaberú veľa miesta. Dajú sa zostreliť až po viacerých zásahoch.
- **Kávové poháre / odpadky** — bežná výplň, veľa ich, ale slabé.

**Boss — Žeriav s demolačnou guľou**
- Iný typ než Meteor Boss: nevznáša sa voľne, ale pohybuje sa po hornom okraji a spúšťa guľu po oblúku.
- Fáza 1: hojdá guľu zo strany na stranu, hráč musí podbehnúť.
- Fáza 2: pustí guľu na zem a tá vyvolá otras, po ktorom padá suť.
- Slabina: lano/kladka nad guľou.

**Špeciálna udalosť — Pizza Delivery**

Obdoba „Meteor Shower" z levelu 1, ale namiesto zhustenia padajúcich objektov priletí **doručovací dron a zhodí pizzu rovno hráčovi na hlavu**. Zásah uberá život.

Priebeh:
1. Hore sa objaví banner **PIZZA DELIVERY** (to isté miesto, kde je v leveli 1 „METEOR SHOWER")
2. Dron priletí zboku a chvíľu **kopíruje pohyb hráča** — dáva najavo, že si ho zameral
3. Zamkne si pozíciu, na zemi sa pod ním ukáže **tieň/značka**, kam pizza dopadne
4. Po krátkej pauze pustí krabicu — kto sa nestihol pohnúť, dostane zásah

**Prečo tá pauza a tieň sú dôležité:** hit, ktorý sa nedá uhnúť, hráč vníma ako nefér a hru zaňho viní. Keď ale dron najprv viditeľne zamieri a až potom pustí, je to **jeho chyba, že sa nepohol** — a to je presne ten pocit, ktorý chceš. Preto musí dron pozíciu zamknúť, nie sledovať hráča až do dopadu; inak sa uhnúť nedá vôbec.

Poznámky k implementácii:
- Spúšťač môže byť ten istý ako pri meteor shower — `targetMilestone` a `nextAllowedShowerTime` v `TargetSpawner`
- `UI.ShowMeteorShowerText()` by stačilo zovšeobecniť tak, aby brala text ako parameter, a používala by sa pre obe udalosti
- Poškodenie rieši `PlayerHealth.TakeDamage(1)`, rovnako ako to už robí `MiniMeteor`

*Rozšírenie, ak by sa to hodilo:* po dopade zostane na zemi mastná škvrna, po ktorej sa kĺže. Bolo by to to isté ako lepkavé škvrny v leveli 5, len s opačným trením.

---

### Level 4 — Renewal Rift (leto, budúcnosť)

**Plošiny** *(odsúhlasené)*
- **Hologramová plošina** — v pravidelnom rytme bliká a mizne. Hráč musí načasovať skok.
- **Antigravitačná plošina** — pri dotyku vystrelí hráča nahor, nahrádza skok.
- **Silové pole** — plošina, cez ktorú sa dá prejsť zdola, ale zhora sa na nej stojí.

Tieto tri spolu fungujú preto, že každá mení iné pravidlo: hologram mení **čas** (kedy tam plošina je), antigravitácia mení **výšku skoku**, silové pole mení **smer, z ktorého sa dá prejsť**. Hráč, ktorý prešiel levelmi 1-3, má pohyb zvládnutý — tu sa mu prepisujú samotné pravidlá, a to je správne finále.

**Čo padá** *(odsúhlasené)*
- **Trosky vesmírnej lode** — pomalé a veľké, dajú sa použiť ako dočasné úkryty.
- **Satelity** — po zostrelení vybuchnú a zničia všetko okolo seba.

**Špeciálna udalosť — UFO** *(odsúhlasené)*

Raz za čas priletí UFO, rovnaký princíp ako Pizza Delivery v leveli 3. Banner hore, potom si UFO hráča zameria a udrie.

Priebeh:
1. Banner **UFO INCOMING**
2. UFO priletí zhora, chvíľu kopíruje pohyb hráča a **zamkne si pozíciu**
3. Pod ním sa rozsvieti kruh únosového lúča — hráč vidí, kam presne dopadne
4. Lúč sa zapne. Kto v ňom stojí, je **ťahaný nahor a stráca život**; dostať sa von sa dá behom alebo skokom mimo kruhu

Oproti pizze je to iný typ hrozby: pizza je jeden úder, ktorému buď uhneš alebo nie, kým lúč **trvá** a musíš sa z neho dostať. Preto by mal byť kruh skôr o niečo širší, ale s dlhším varovaním — inak je to len nefér verzia pizze.

*Jednoduchšia verzia, ak by ťahanie hráča robilo problémy:* UFO namiesto lúča vystrelí jednu silnú ranu nadol, teda presne to isté správanie ako pizza, len s iným spritom a zvukom.

**Boss — Materská loď**
- Toto je najväčší skok v obtiažnosti, lebo boss prvýkrát **strieľa po hráčovi**, takže sa musí aj uhýbať, nielen mieriť.
- Fáza 1: strieľa dávky laserov po hráčovej pozícii, medzi dávkami je pauza na streľbu.
- Fáza 2: vypustí menšie UFO, ktoré lietajú okolo a strieľajú (obdoba mini meteoritov).
- Fáza 3: zapne štít a je nezraniteľná, kým hráč nezostrelí generátory po bokoch.
- Slabina: spodný lúč, ktorý sa otvára vždy pred výstrelom.

---

### Špeciálne udalosti — prehľad

Z doterajších rozhodnutí vychádza pekný vzorec: **každý level má jednu vlastnú udalosť**, ktorá občas preruší bežné hranie a spustí sa rovnakým mechanizmom (banner hore + spúšťač po určitom počte zostrelených cieľov).

| Level | Udalosť | Čo robí | Stav |
|---|---|---|---|
| 1 — Meteor Madness | METEOR SHOWER | zahustí padajúce meteority | hotové |
| 2 — Snowfall Survival | ? | ? | návrh nižšie |
| 3 — Modern Mayhem | PIZZA DELIVERY | dron zhodí pizzu na hráča | odsúhlasené |
| 4 — Renewal Rift | UFO INCOMING | únosový lúč ťahá hráča nahor | odsúhlasené |
| 5 — Bubble (DLC) | ? | ? | návrh nižšie |

**Návrh pre level 2 — BLIZZARD:** na pár sekúnd sa zhorší viditeľnosť a vietor tlačí hráča do strany. Na šmykľavom ľade je to nepríjemné presne tak akurát, a nepotrebuje to nový objekt — stačí efekt a sila pôsobiaca na hráča.

**Návrh pre level 5 — HAPPY HOUR:** naraz priletí niekoľko bubble tea a zaplavia zem lepkavými škvrnami. Hráčovi sa rázom zmenší priestor a musí sa prestriedať na zvyšné čisté miesta — čiže udalosť neublíži priamo, ale spustí lavínu ôs.

Všimni si rozdiel medzi nimi: meteor shower a happy hour **zhusťujú** to, čo už v leveli je, kým pizza a UFO **priamo mieria na hráča**. Oba typy sú dobré, len je fajn vedieť, ktorý kedy použiješ — mierená udalosť je vždy výraznejšia, takže by nemala chodiť príliš často.

### Pozadia

**Level 1 — Meteor Madness (jeseň, praveká)**
- Jaskyňa a pravekí krajina za ňou, v diaľke sopky
- Nočná obloha s meteorickým rojom — rovno vysvetľuje, prečo padajú meteority
- Paleta: oranžová, hrdzavá, hnedá, tmavomodrá obloha
- Detail do pohybu: padajúce lístie, dym zo sopiek

**Level 2 — Snowfall Survival (zima, doba ľadová)**
- Ľadová jaskyňa — tá istá jaskyňa ako v leveli 1, ale zamrznutá. Pekne to ukáže, že prešiel čas.
- Cencúle na strope, zamrznuté jazero v pozadí, polárna žiara vysoko hore
- Paleta: bledomodrá, biela, tyrkysová, fialové tiene
- Detail do pohybu: padajúci sneh, para z hráčovho dychu

**Level 3 — Modern Mayhem (jar, moderná doba)**
- Mesto — mrakodrapy, stavenisko so žeriavmi (rovno predstaví bossa), billboardy, semafory
- V diaľke vidno miesto, kde bola jaskyňa, teraz zastavané
- Paleta: sivá, betónová, k tomu jarná zeleň a neónové reklamy
- Detail do pohybu: dážď, blikajúce okná, prelietavajúce lietadlo

**Level 4 — Renewal Rift (leto, budúcnosť)**
- Vesmírna stanica na obežnej dráhe, pod hráčom vidno planétu
- V pozadí trhlina (rift), z ktorej prilietajú UFO
- Paleta: tmavá s neónovou fialovou a tyrkysovou, veľa svetelných zdrojov
- Detail do pohybu: prelietavajúce lode, blikajúce hologramy

**Level 5 — Bubble (DLC)**
- Interiér obrovskej bubble tea predajne — hráč je zmenšený, takže poháre a slamky sú ako budovy
- V pozadí pult, menu tabuľa, obrie poháre
- Paleta: pastelová — ružová, mätová, krémová. Zámerne najveselší level z celej hry.
- Detail do pohybu: stúpajúce bublinky, para z čaju

**Level 6 — DLC**
- Nerozhodnuté

### Zbrane

**Odporúčanie: raketomet nechať, ale meniť mu vzhľad a strelu podľa levelu.**

Dinosaurus s raketometom je najzapamätateľnejšia vec na celej hre — je to vtip, ktorý funguje práve preto, že do praveku nepatrí. To by som nerušila. Zároveň ale platí, že ak je vo všetkých šiestich leveloch úplne tá istá zbraň, levely sa od seba líšia len pozadím.

Preto navrhujem strednú cestu: **mechanika zostáva vo všetkých leveloch rovnaká** (mierenie myšou, 15 nábojov, reload na R), mení sa len sprite zbrane, sprite strely a efekt zásahu.

| Level | Zbraň | Strela |
|---|---|---|
| 1 — Meteor Madness | Raketomet | Raketa (súčasný stav) |
| 2 — Snowfall Survival | Mrazomet | Ľadová guľa |
| 3 — Modern Mayhem | Klincovačka zo staveniska | Klinec |
| 4 — Renewal Rift | Plazmová puška | Laserový lúč |
| 5 — Bubble (DLC) | Bublifuk | Bublina / tapioka |
| 6 — DLC | ? | ? |

**Prečo sa to oplatí:** v kóde je to lacné. `GunController` má jedno pole `bulletPrefab` a jeden `Animator`, takže na nový typ zbrane stačí iný prefab a iný sprite — žiadna nová logika. Za pár minút práce dostaneš level, ktorý pôsobí úplne inak.

**Čo by som nemenila:** počet nábojov, rýchlosť streľby ani reload. Keby každý level ovládaš inak, hráč sa musí zakaždým učiť odznova a stratí istotu, ktorú si vybudoval.

**Ak by si predsa chcela jeden rozdiel navyše**, najlepší kandidát je plazmová puška v leveli 4 — mohla by prestreliť viac cieľov naraz. Sedí to k téme budúcnosti a je to odmena za to, že hráč došiel najďalej.

### Nadväznosť tém

Jeseň → zima → jar → leto je zároveň praveká → ľadová → moderná → budúcnosť, čiže cesta časom.

Ak sa to má dať prečítať bez vysvetľovania, pomôže držať **jedno miesto naprieč levelmi** — tá istá jaskyňa, ktorú vidíš v leveli 1, je v leveli 2 zamrznutá a v leveli 3 zastavaná mestom. Hráč tak nevidí štyri nesúvisiace levely, ale jedno miesto v štyroch dobách.

Stupňovanie obtiažnosti, ktoré z návrhu vychádza:
1. **Meteor Madness** — nauč sa mieriť a skákať
2. **Snowfall Survival** — nespoľahni sa na to, že plošina zostane pod tebou
3. **Modern Mayhem** — plošina ťa sama posúva, cieľ sa hýbe inak než ty
4. **Renewal Rift** — po prvýkrát strieľajú aj po tebe
