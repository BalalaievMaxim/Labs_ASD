// Виконав Балалаєв Максим, ІМ-43
// Варіант із обчисленням членів ряду й суми на рекурсивному поверненні

#include <stdio.h>

typedef struct
{
    double sum;
    double last_F;
} Result;

Result sum_ascending(const unsigned n, const double x)
{
    if (n == 1)
        return (Result){.sum = 1, .last_F = 1};
    else
    {
        Result prev = sum_ascending(n - 1, x);
        double F = -prev.last_F * x * (2 * n - 3) / (2 * n - 2);
        
        return (Result){.sum = prev.sum + F, .last_F = F};
    }
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

    double result = sum_ascending(n, x).sum;
    printf("Result: %lf\n", result);

    return 0;
}