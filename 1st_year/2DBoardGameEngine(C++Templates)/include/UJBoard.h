#ifndef UJBOARD_H
#define UJBOARD_H

#include "UJBoardRow.h"
#include <iostream>

template <typename T>
class UJBoard {
public:
    UJBoard();
    UJBoard(int r, int c);
    UJBoard(const UJBoard<T>& objBoard);
    UJBoard<T>& operator=(const UJBoard<T>& objBoard);
    ~UJBoard();

    UJBoardRow<T>& operator[](int index);
    void operator()(int seed);

    template <typename U>
    friend std::ostream& operator<<(std::ostream& os, const UJBoard<U>& objRHS);

private:
    int GenRandNum(int LowerBound, int UpperBound);
    void Swap(T& a, T& b);

    UJBoardRow<T>** board;
    int rows, cols;
};

#include "UJBoard.imp"

#endif
