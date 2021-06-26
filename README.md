# Short introduction
The game is playable in a single-player way (number of players equals 1) or a maximum of 8 local players. The user can add a number of questions between 1 and 50 (OpenTrivia gives a maximum of 50 questions from one response). Finally, a category can be selected.

When the user sent the inputs, a short validation happening by the UIController.

When the form has been successfully filled, the backend starts to download the questions async way and in the meantime the loading screen is visible.

After the questions are collected, the backend sends them to the GameController and it initializes the GameState and sends the information to the UIController. The UIController shows the QuizPanel with the data.

The process of the quiz: every player has a timer separated from each other. The timer applies to the entire quiz, not just one question (players get 10s for every question). The actual player can select an answer, when the answer is selected, the next player gets the turn with the same question. If a player has run out of time, then that player score will be saved and in the other rounds that player cannot choose answers.

The game has been finished if there are no other questions or all of the players have run out of time. At the end of the game, the result will be visible with the scores of the players.

