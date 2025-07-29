#include "UJImage.h"

#include <fstream>  // For std::ofstream

// Default constructor: initialize with default dimensions and color
UJImage::UJImage() : rows(DefaultRows), cols(DefaultCols) {
    Alloc();
    for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
            pixels[r][c] = DefaultColour;
}

// Parameterized constructor: initialize with custom size and default color
UJImage::UJImage(int r, int c) : rows(r), cols(c) {
    Alloc();
    for (int i = 0; i < rows; i++)
        for (int j = 0; j < cols; j++)
            pixels[i][j] = DefaultColour;
}

// Destructor: deallocates dynamic memory
UJImage::~UJImage() {
    Dealloc();
}

// Copy constructor: performs deep copy of another UJImage
UJImage::UJImage(const UJImage& other) : rows(other.rows), cols(other.cols) {
    Alloc();
    for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
            pixels[r][c] = other.pixels[r][c];
}

// Assignment operator: deep copy with self-assignment check
UJImage& UJImage::operator=(const UJImage& other) {
    if (this == &other) return *this;
    Dealloc();
    rows = other.rows;
    cols = other.cols;
    Alloc();
    for (int r = 0; r < rows; ++r)
        for (int c = 0; c < cols; ++c)
            pixels[r][c] = other.pixels[r][c];
    return *this;
}

// Equality operator: compares pixel-by-pixel
bool UJImage::operator==(const UJImage& other) const {
    if (rows != other.rows || cols != other.cols) return false;
    for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
            if (pixels[r][c].red != other.pixels[r][c].red ||
                pixels[r][c].green != other.pixels[r][c].green ||
                pixels[r][c].blue != other.pixels[r][c].blue)
                return false;
    return true;
}

// Operator to access a row (used for [][] access)
Pixel* UJImage::operator[](int row) {
    if (row < 0 || row >= rows) {
        cerr << "Row index out of bounds" << endl;
        exit(ErrBounds);
    }
    return pixels[row];
}

// Operator to access a single pixel (used for (row, col) access)
Pixel& UJImage::operator()(int row, int col) {
    if (row < 0 || row >= rows || col < 0 || col >= cols) {
        cerr << "Index out of bounds" << endl;
        exit(ErrBounds);
    }
    return pixels[row][col];
}

// Post-increment operator: increments each RGB value and wraps at 255
UJImage UJImage::operator++(int) {
    for (int r = 0; r < rows; r++) {
        for (int c = 0; c < cols; c++) {
            pixels[r][c].red = (pixels[r][c].red + 1) % 256;
            pixels[r][c].green = (pixels[r][c].green + 1) % 256;
            pixels[r][c].blue = (pixels[r][c].blue + 1) % 256;
        }
    }
    return *this;
}

// Addition operator: averages RGB values pixel-by-pixel
UJImage UJImage::operator+(const UJImage& other) const {
    if (rows != other.rows || cols != other.cols) {
        cerr << "Images must be of the same dimensions" << endl;
        exit(ErrDimensions);
    }
    UJImage result(rows, cols);
    for (int r = 0; r < rows; ++r) {
        for (int c = 0; c < cols; ++c) {
            result.pixels[r][c].red = (pixels[r][c].red + other.pixels[r][c].red) / 2;
            result.pixels[r][c].green = (pixels[r][c].green + other.pixels[r][c].green) / 2;
            result.pixels[r][c].blue = (pixels[r][c].blue + other.pixels[r][c].blue) / 2;
        }
    }
    return result;
}

// Stream insertion operator: prints image in ASCII PPM format
ostream& operator<<(ostream& os, const UJImage& img) {
    os << "P3" << endl;
    os << img.cols << " " << img.rows << endl;
    os << 255 << endl;
    for (int r = 0; r < img.rows; ++r) {
        for (int c = 0; c < img.cols; ++c) {
            os << img.pixels[r][c].red << " "
               << img.pixels[r][c].green << " "
               << img.pixels[r][c].blue << " ";
        }
        os << endl;
    }
    return os;
}

// Allocate 2D array for pixels
void UJImage::Alloc() {
    pixels = new Pixel*[rows];
    for (int r = 0; r < rows; ++r)
        pixels[r] = new Pixel[cols];
}

// Deallocate 2D pixel array
void UJImage::Dealloc() {
    for (int r = 0; r < rows; ++r)
        delete[] pixels[r];
    delete[] pixels;
}

void UJImage::SaveToFile(const std::string& filename) const {
    std::ofstream ofs(filename);
    if (!ofs.is_open()) {
        std::cerr << "Error: Could not open file " << filename << " for writing." << std::endl;
        return;
    }

    // Write the PPM header
    ofs << "P3" << std::endl;              // ASCII PPM magic number
    ofs << cols << " " << rows << std::endl;  // Image width and height
    ofs << 255 << std::endl;               // Maximum color value

    // Write pixel data
    for (int r = 0; r < rows; ++r) {
        for (int c = 0; c < cols; ++c) {
            ofs << pixels[r][c].red << " "
                << pixels[r][c].green << " "
                << pixels[r][c].blue << " ";
        }
        ofs << std::endl;
    }

    ofs.close();
}
