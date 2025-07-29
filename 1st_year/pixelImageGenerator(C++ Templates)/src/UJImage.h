#ifndef UJIMAGE_H
#define UJIMAGE_H

#include <iostream>
using namespace std;

// Struct to represent a single RGB pixel
struct Pixel {
    int red;
    int green;
    int blue;
};

// Enum for error codes used in the class
enum ErrorCodes {
    ErrBounds = -2,
    ErrDimensions
};

// UJImage class representing a 2D RGB image
class UJImage {
private:
    Pixel** pixels;  // 2D dynamic array of pixels
    int rows;
    int cols;

    // Helper methods for memory management
    void Alloc();
    void Dealloc();

public:
    // Default image size and color
    static const int DefaultRows = 20;
    static const int DefaultCols = 20;
    static constexpr Pixel DefaultColour = {255, 255, 255};

    // Constructors and destructor
    UJImage();
    UJImage(int, int);
    ~UJImage();

    // Copy constructor and assignment operator
    UJImage(const UJImage&);
    UJImage& operator=(const UJImage&);

    // Operator overloads for image operations
    bool operator==(const UJImage&) const;           // Compare two images
    Pixel* operator[](int);                          // Access row of pixels
    Pixel& operator()(int, int);                     // Access single pixel
    UJImage operator++(int);                         // Increment each pixel's RGB values
    UJImage operator+(const UJImage&) const;         // Add two images (average RGB values)

    // Overloaded stream insertion operator to output in PPM format
    friend ostream& operator<<(ostream&, const UJImage&);
	
	void SaveToFile(const std::string& filename) const;

};

#endif // UJIMAGE_H