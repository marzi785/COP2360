using System;
using System.Collections.Generic;
using System.Globalization;

namespace ContractorApp
{
    // Base class: Contractor
    public class Contractor
    {
        // Fields (kept private to enforce encapsulation)
        private string _name = string.Empty;
        private int _contractorNumber = 0;
        private DateTime _startDate;

        // Constructors
        public Contractor() : this("Unknown", 0, DateTime.Today) { }

        public Contractor(string name, int contractorNumber, DateTime startDate)
        {
            Name = name;
            ContractorNumber = contractorNumber;
            StartDate = startDate;
        }

        // Accessors/Mutators (Properties with validation)
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name can't be empty.");
                _name = value.Trim();
            }
        }

        public int ContractorNumber
        {
            get => _contractorNumber;
            set
            {
                if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Contractor number must be non-negative.");
                _contractorNumber = value;
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value > DateTime.Today)
                    throw new ArgumentException("Start date can't be in the future.");
                _startDate = value.Date;
            }
        }

        public override string ToString()
        {
            return $"{Name} (#{ContractorNumber}) — Start: {StartDate:yyyy-MM-dd}";
        }
    }

    // Derived class: Subcontractor
    public class Subcontractor : Contractor
    {
        // Shift: 1 = Day, 2 = Night
        private int _shift;
        private double _hourlyPayRate;

        // Constructors
        public Subcontractor()
            : this("Unknown", 0, DateTime.Today, 1, 0.0) { }

        public Subcontractor(
            string name,
            int contractorNumber,
            DateTime startDate,
            int shift,
            double hourlyPayRate
        ) : base(name, contractorNumber, startDate)
        {
            Shift = shift;
            HourlyPayRate = hourlyPayRate;
        }
        // Accessors/Mutators
        /// Shift must be 1 (Day) or 2 (Night).
        public int Shift
        {
            get => _shift;
            set
            {
                if (value != 1 && value != 2)
                    throw new ArgumentOutOfRangeException(nameof(Shift), "Shift must be 1 (Day) or 2 (Night).");
                _shift = value;
            }
        }

        public double HourlyPayRate
        {
            get => _hourlyPayRate;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(HourlyPayRate), "Hourly pay rate can't be negative.");
                _hourlyPayRate = value;
            }
        }
        /// Computes pay for the given hours. Night shift (2) gets a 3% differential.
        /// Returns a float per requirement.
        public float ComputePay(float hoursWorked)
        {
            if (hoursWorked < 0f)
                throw new ArgumentOutOfRangeException(nameof(hoursWorked), "Hours can't be negative.");

            double basePay = HourlyPayRate * hoursWorked;
            double multiplier = (Shift == 2) ? 1.03 : 1.00; // 3% bump for night shift
            double total = basePay * multiplier;
            return (float)total;
        }

        public override string ToString()
        {
            string shiftLabel = Shift == 2 ? "Night (2)" : "Day (1)";
            return base.ToString() + $" — Shift: {shiftLabel}, Rate: {HourlyPayRate:C}";
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Subcontractor Factory ===");
            Console.WriteLine("Create as many subcontractors as you like. Type 'q' at name prompt to finish.\n");

            var subs = new List<Subcontractor>();

            while (true)
            {
                try
                {
                    string name = Prompt("Contractor name (or 'q' to quit): ");
                    if (name.Equals("q", StringComparison.OrdinalIgnoreCase))
                        break;

                    string number = Prompt("Contractor number: ");
                    if (!int.TryParse(number, out int contractorNumber))
                        throw new ArgumentException("Contractor number must be an integer.");

                    DateTime start = ReadDate("Start date (YYYY-MM-DD): ");

                    int shift = ReadInt("Shift (1=Day, 2=Night): ", v => v == 1 || v == 2,
                        "Shift must be 1 or 2.");

                    double rate = ReadDouble("Hourly pay rate: ", v => v >= 0,
                        "Hourly rate must be >= 0.");

                    var sub = new Subcontractor(name, contractorNumber, start, shift, rate);
                    subs.Add(sub);

                    Console.WriteLine("Subcontractor created:");
                    Console.WriteLine("   " + sub);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Retry entry.\n");
                }
            }

            if (subs.Count == 0)
            {
                Console.WriteLine("\nNo subcontractors created. Exiting...");
                return;
            }

            Console.WriteLine($"\nYou created {subs.Count} subcontractor(s).");
            Console.WriteLine("Let’s compute pay for each based on hours worked this period.\n");

            foreach (var sub in subs)
            {
                Console.WriteLine(sub);
                float hours = ReadFloat("Hours worked this period: ", v => v >= 0f,
                    "Hours must be >= 0.");

                float pay = sub.ComputePay(hours);
                Console.WriteLine($"   -> Computed Pay: {pay.ToString("C", CultureInfo.CurrentCulture)}");
                Console.WriteLine();
            }

            Console.WriteLine("Subcontractor processing complete. Goodbye!");
        }

        // -------- Helper input methods (basic validation) --------

        static string Prompt(string label)
        {
            Console.Write(label);
            return Console.ReadLine() ?? string.Empty;
        }

        static DateTime ReadDate(string label)
        {
            while (true)
            {
                Console.Write(label);
                var input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var dt))
                    return dt;

                Console.WriteLine("Invalid date. Please use YYYY-MM-DD.\n");
            }
        }

        static int ReadInt(string label, Func<int, bool> predicate, string errorMsg)
        {
            while (true)
            {
                Console.Write(label);
                var input = Console.ReadLine();
                if (int.TryParse(input, out var v) && predicate(v))
                    return v;

                Console.WriteLine(errorMsg + "\n");
            }
        }

        static double ReadDouble(string label, Func<double, bool> predicate, string errorMsg)
        {
            while (true)
            {
                Console.Write(label);
                var input = Console.ReadLine();
                if (double.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var v) && predicate(v))
                    return v;

                Console.WriteLine(errorMsg + "\n");
            }
        }

        static float ReadFloat(string label, Func<float, bool> predicate, string errorMsg)
        {
            while (true)
            {
                Console.Write(label);
                var input = Console.ReadLine();
                if (float.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var v) && predicate(v))
                    return v;

                Console.WriteLine(errorMsg + "\n");
            }
        }
    }
}
