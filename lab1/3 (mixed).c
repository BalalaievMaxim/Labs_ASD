// Виконав Балалаєв Максим, ІМ-43
// Варіант із обчисленням членів ряду на рекурсивному спуску, а суми - на поверненні

#include <stdio.h>

double sum_mixed(const unsigned n, const double x, const int i, const double prev_F)
{
    if (i > n)
        return 1;

    double F = -prev_F * x * (2 * i - 3) / (2 * i - 2);
    return F + sum_mixed(n, x, i + 1, F);
}
        
double sum_wrapper(const unsigned n, const double x)
{
    return sum_mixed(n, x, 2, 1);
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