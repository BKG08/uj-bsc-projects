#ifndef UJBOARDROW_H
#define UJBOARDROW_H

#include <iostream>

template <typename T>
class UJBoardRow {
private:
    T* row;
    int Numrows;

public:
    UJBoardRow();
    UJBoardRow(int s);
    UJBoardRow(const UJBoardRow<T>& objRow);
    UJBoardRow<T>& operator=(const UJBoardRow<T>& objRow);
    ~UJBoardRow();

    T& operator[](int index);

    template <typename U>
    friend std::ostream& operator<<(std::ostream& os, const UJBoardRow<U>& objRHS);
};

#include "UJBoardRow.imp"

#endif
