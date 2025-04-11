// Виконав Балалаєв Максим, ІМ-43

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct linked_list
{
    char data;
    struct linked_list *next;
} Node;

Node *add_to_start(Node *head, char data)
{
    Node *new_node = (Node *)malloc(sizeof(Node));
    new_node->data = data;
    new_node->next = head;

    return new_node;
}

Node *remove_node(Node *head, Node *node, Node *prev)
{
    if (head == node)
    {
        head = head->next;
        free(node);
        return head;
    }

    if (prev != NULL)
        prev->next = node->next;

    free(node);
    return head;
}

Node *init_list(const char *data)
{
    Node *head = NULL;

    const unsigned int len = strlen(data);
    for (unsigned int i = len; i > 0; i--)
    {
        head = add_to_start(head, data[i - 1]);
    }

    return head;
}

void print_list(Node *head)
{
    Node *current = head;
    while (current != NULL)
    {
        printf("%c", current->data);
        current = current->next;
    }
    printf("\n");
}

void delete_list(Node *head)
{
    while (head != NULL)
    {
        Node *temp = head;
        head = head->next;
        free(temp);
    }
}

int is_digit(char c)
{
    return c >= '0' && c <= '9';
}

int is_list_numeric(Node *head)
{
    Node *current = head;
    while (current != NULL)
    {
        if (!is_digit(current->data))
            return 0;
        current = current->next;
    }
    return 1;
}

Node *recompose(Node *head)
{
    Node *current = head;
    Node *prev = NULL;

    if (is_list_numeric(head))
        return head;

    while (current != NULL)
    {
        Node *next = current->next;

        if (current->data >= '0' && current->data <= '9')
        {
            const char digit = current->data;
            head = remove_node(head, current, prev);
            head = add_to_start(head, digit);
        }
        else
        {
            prev = current;
        }

        current = next;
    }

    return head;
}

int main()
{
    unsigned int n;
    printf("Enter the number of elements (n): ");
    scanf("%u", &n);

    char *data = (char *)malloc(n + 1);
    if (!data)
    {
        printf("Memory allocation failed\n");
        return 1;
    }

    printf("Enter the elements: ");
    getchar();
    fgets(data, n + 1, stdin);

    Node *linked_list = init_list(data);
    printf("Initial list: ");
    print_list(linked_list);

    linked_list = recompose(linked_list);
    printf("Recomposed list: ");
    print_list(linked_list);

    delete_list(linked_list);
    free(data);

    return 0;
}
