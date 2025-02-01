EXTERNAL ChangeBackground(imageName)
EXTERNAL ToggleGameplay(shouldItStart)
EXTERNAL AskInput(varName, placeholder, continueTag)
EXTERNAL Scene_Home()

VAR meyir_mood = ""
VAR playerName = ""

~ ChangeBackground("camp")

"Look at them," Meyir said, pointing at the camp down in the valley. "Traitors of our cause."
*   "Don't speak like they do[."]," I replied.
    Meyir turned to me, her rage at the Jevarrs brewing with confusion.
    "I am nothing like them," she responded, pulling on the reins to turn Zhan around to face me.
    * * ["Yet. Continue like this and you'll drown in hatred."]"Yet," I responded. "Continue like this and you'll drown in hatred."
        ~ ChangeBackground("camp2")
        Meyir readies herself to bark an reply to me, but seeing my expression, bites her tongue.
        "You are wise, Khan." She responds instead, suffocating her rage.
        She turned Zhan back around to face the valley and pointed at the largest yurt.
        ~ meyir_mood = "suppressed_rage"
    * * ["The Winged Jevvars are like you and me. It's Khan Lengu who's poisoned their hearts."]""The Winged Jevvars are like you and me," I argued. "It's Khan Lengu who's poisoned their hearts."
        ~ ChangeBackground("camp2")
        She furrows her brows, clearly thinking. I always knew what will get through to her.
        "We'll flood the steppe with Lengu's blood," she finally says, after a long pause.
        "And free our sisters," we said, almost in unison.
        ~ meyir_mood = "zen_blames_Lengu"
*   "Their time is coming[."]," I responded.
    "That it is," Meiyr said, with a bloodthirsty glare aimed at the yurts down in the valley.
    "When they gather and start to move to their next keshik, we'll get them."
    Her eyes were gleaming with ideas.
    * * "I'll take care of their scouts personally[."]," I supported.
        ~ ChangeBackground("camp2")
        She looked like a child at Nauryz, a smile wider than the Amu in spring.
        "You were always the best shot," she said.
        ~ meyir_mood = "excited_for_attack"
    * * "We need to study them for longer[."]," I decided.
        ~ ChangeBackground("camp2")
        Her face shifted to confusion as she pleaded, "They'll spot us if we return here! We have to take a shot!"
        "And they'll slaughter us if we attack with no preparation," I cut her off.
        My expression showed that the discussion was over and she nodded. "Yes, Khan."
        ~ meyir_mood = "suppressed_rage"

-   "Looks like they're heading out to herd," she pointed, urgency in her voice. "If they-"
*   "Then we're going back," I cut her off.
    ~ ChangeBackground("steppe")
    Momentarily, we were riding down the other side of the hill.
    Meyir's expression showed <>

    { meyir_mood == "suppressed_rage":
        anger.
        She gritted her teeth and held onto the reins tight, but said nothing.
    }
    { meyir_mood == "excited_for_attack": 
        eagerness.
        She kept one hand on her favorite knife on her belt.
    }
    { meyir_mood == "zen_blames_Lengu":
        surprising peace.
        She smiled at me when I glanced at her.
    }

    "Mind if I ask you a question, Khan?" she interrupted my thoughts.
    I nod.
    "What does your name mean?" she asks unexpectedly.
    There's a genuineness to her voice. 
    ~ AskInput("playerName", "Enter your name...", "name_given")
    -> END

=== name_given ===
She continues: "I mean, it is {playerName}." 
"What does it mean in the tongue of our people?"
I turn to her, slowly. 
"{playerName} was given to me by my mother. It was her name as well."
"You should remember these things, Meyir." I say sternly.
"Yes, Khan {playerName}," Meyir said, without further challenge in her voice.
We rode the rest of the way home in silence.
~ ChangeBackground("canyon")
~ Scene_Home()
~ ToggleGameplay(true)
-> END

=== red_house ===
We've gotten this place during our last raid on the riverfolk.
Most riders live in yurts.
As Khan, I decided to take one of the stone houses.
Normally, I wouldn't. Riverfolk houses aren't better in any way.
This time, however...
*   I needed warmth for my injury.
*   I wished to study their living conditions.
-   And so I enter into my house.
    Nothing seems amiss.
    -> END

=== green_house ===
I enter the largest house.
We've been using this as a common house.
The oldest raiders huddled on the floor in front of a large map.
They held tea pialas in their hands.
Seeing me approach, they quickly handed one to me.
*   I took the piala.
*   I sternly shook my head.
-   I sat down near them, studying the map.
    "Khan {playerName}," said Bortu, one of the oldest, most reliable raiders raiders.
    "The Winged Ones have been quiet today."
    -> END