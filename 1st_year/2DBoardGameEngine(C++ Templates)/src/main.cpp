#include "UJBoard.h"
#include <iostream>

int main() {
    // Create an integer board
    UJBoard<int> intBoard(3, 3);

    // Assign values
    intBoard[0][0] = 1;
    intBoard[0][1] = 2;
    intBoard[0][2] = 3;
    intBoard[1][0] = 4;
    intBoard[1][1] = 5;
    intBoard[1][2] = 6;
    intBoard[2][0] = 7;
    intBoard[2][1] = 8;
    intBoard[2][2] = 9;

    std::cout << "Original board:\n" << intBoard;

    // Randomize the board with a seed
    intBoard(100);
    std::cout << "Randomized board:\n" << intBoard;

    // Character board example
    UJBoard<char> charBoard(2, 5);
    charBoard[0][0] = 'A';
    charBoard[0][1] = 'B';
    charBoard[0][2] = 'C';
    charBoard[0][3] = 'D';
    charBoard[0][4] = 'E';
    charBoard[1][0] = 'F';
    charBoard[1][1] = 'G';
    charBoard[1][2] = 'H';
    charBoard[1][3] = 'I';
    charBoard[1][4] = 'J';

    std::cout << "Character board:\n" << charBoard;

    return 0;
}
