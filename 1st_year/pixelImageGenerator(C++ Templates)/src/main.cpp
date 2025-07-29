#include <iostream>
#include "UJImage.h"
using namespace std;

int main() {
    // Create a UJImage object for the smiley face
    UJImage smiley;

    // Draw eyes (black)
    smiley(5, 5) = {0, 0, 0};     // Left eye
    smiley(5, 14) = {0, 0, 0};    // Right eye

    // Draw smile (black)
    for (int c = 6; c <= 13; ++c)
        smiley(14, c) = {0, 0, 0};
    smiley(13, 6) = {0, 0, 0};
    smiley(13, 13) = {0, 0, 0};

    // Output the image as a PPM format
    cout << "Smiley Face Image Output (PPM format):" << endl;
    cout << smiley << endl;
	
	smiley.SaveToFile("smiley.ppm");

    return 0;
}