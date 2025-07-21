using System;
class Program
{
    static void Main(string[] args)
    {
        //Ask for the first number
        Console.WriteLine("Enter the first number: ");
        string input1 = Console.ReadLine() ?? string.Empty; // Use null-coalescing operator to handle null input

        //Ask for the second number
        Console.WriteLine("Enter the second number: ");
        string input2 = Console.ReadLine() ?? string.Empty; // Use null-coalescing operator to handle null input

        try
        {
            // Convert inputs to integers
            int number1 = Convert.ToInt32(input1);
            int number2 = Convert.ToInt32(input2);

            int result = Divide(number1, number2);
            Console.WriteLine($"The result of dividing {number1} by {number2} is {result}.");
        }

        // Handle specific exceptions
        // FormatException is thrown when the input is not a valid integer
        catch (FormatException ex)
        {
            Console.WriteLine("Invalid input. One or both inputs are not valid integers.");
            Console.WriteLine($"Error details: {ex.Message}");
        }

        // DivideByZeroException is thrown when the second number is zero
        catch (DivideByZeroException ex)
        {
            Console.WriteLine("Error: Division by zero is not allowed.");
            Console.WriteLine($"Error details: {ex.Message}");
        }

        // OverflowException is thrown when the input is too large or too small for an Int32
        catch (OverflowException ex)
        {
            Console.WriteLine("Error: One of the numbers is too large or too small for an Int32.");
            Console.WriteLine($"Error details: {ex.Message}");
        }

        // Catch any other unexpected exceptions
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred.");
            Console.WriteLine($"Error details: {ex.Message}");
        }

        // Wait for user input before closing
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static int Divide(int a, int b)
    {
        return a / b;
    }
}