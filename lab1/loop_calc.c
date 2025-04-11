// Виконав Балалаєв Максим, ІМ-43
// Варіант із обчисленням у циклі for для тестування

#include <stdio.h>

double sum_loop(const unsigned n, const double x)
{
    double sum = 1, F = 1;
    for (int i = 2; i <= n; i++)
    {
        F *= -x * (2 * i - 3) / (2 * i - 2);
        sum += F;
    }
    
    return sum;
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

    double result = sum_loop(n, x);
    printf("Result: %lf\n", result);

    return 0;
}