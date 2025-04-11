// Виконав Балалаєв Максим, ІМ-43
// Варіант із обчисленням членів ряду й суми на рекурсивному спуску

#include <stdio.h>

double sum_descending(const unsigned n, const double x, const int i, const double prev_F, double sum)
{
    double F;
    if (i > n)
    {
        return sum;
    }
    else if (i == 1)
    {
        F = 1;
    }
    else
    {
        F = -prev_F * x * (2 * i - 3) / (2 * i - 2);
    }
    sum += F;

    return sum_descending(n, x, i + 1, F, sum);
}

double sum_wrapper(const unsigned n, const double x)
{
    return sum_descending(n, x, 1, 1, 0);
}

int main()
{
    double x;
    printf("Enter x: ");
    scanf("%lf", &x);

    int n;
    printf("Enter n: ");
    scanf("%i", &n);

    if (n < 1 || x > 1 || x < -1)
    {
        printf("Incorrect input\n");
        return 1;
    }

    double result = sum_wrapper(n, x);
    printf("Result: %lf\n", result);

    return 0;
}