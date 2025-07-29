# UJImage Smiley Generator

This C++ project generates a simple pixelated smiley face and outputs it in the PPM (Portable Pixmap) image format using dynamic memory and operator overloading.

## Features

- Dynamic 2D array to store RGB pixels (`Pixel` struct)
- Operator overloading:
  - `[]` and `()` for pixel access
  - `==` for image comparison
  - `++` for RGB increment cycling
  - `+` for pixel-wise blending
  - `<<` to output a PPM image
- ASCII-based PPM image generation
- Hardcoded image layout for a 20×20 smiley face

## How It Works

- `UJImage smiley;` creates a white image (20x20)
- Black pixels (`{0,0,0}`) are placed for:
  - Eyes at `(5,5)` and `(5,14)`
  - Smile from `(13,6)` to `(13,13)` and `(14,6)` to `(14,13)`
- The final image is printed in ASCII PPM format to `stdout`

## Build & Run

### Compile
```bash
g++ main.cpp UJImage.cpp -o ujimage
```

### Run & Save
```bash
./ujimage > smiley.ppm
```

You can then open `smiley.ppm` using:
- GIMP
- IrfanView
- Paint.NET (with plugin)
- Any PPM-supporting tool

## Output Preview (PPM Format Snippet)
```
P3
20 20
255
255 255 255 ...
255 255 255 ...
0 0 0 255 255 255 ...    # Eye and smile pixels
...
```

## Educational Concepts

- Dynamic memory management in C++
- Operator overloading for intuitive image operations
- Image encoding in text-based formats
- 2D grid rendering and pixel manipulation
