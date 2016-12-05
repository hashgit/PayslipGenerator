using System.Collections.Generic;
using System.Linq;

namespace PayslipGenerator.Lib.Calculators
{
    public class TaxCalculator : ITaxCalculator
    {
        private readonly IList<Tier> _tiers = new List<Tier>();

        public TaxCalculator()
        {
            _tiers.Add(new Tier { StartValue = 0, BaseTax = 0, Factor = 0 });
            _tiers.Add(new Tier { StartValue = 18201, BaseTax = 0, Factor = 0.19m });
            _tiers.Add(new Tier { StartValue = 37001, BaseTax = 3572, Factor = 0.325m });
            _tiers.Add(new Tier { StartValue = 80001, BaseTax = 17547, Factor = 0.37m });
            _tiers.Add(new Tier { StartValue = 180001, BaseTax = 54547, Factor = 0.45m });
        }

        public decimal Calculate(InputData data)
        {
            var tier = _tiers.OrderByDescending(o => o.StartValue)
                .First(t => data.AnnualSalary >= t.StartValue);

            if (tier.StartValue == 0) return 0;
            var tax = decimal.Round(tier.BaseTax + (data.AnnualSalary - tier.StartValue - 1)*tier.Factor);
            return tax;
        }
    }

    internal class Tier
    {
        public int StartValue { get; set; }
        public decimal BaseTax { get; set; }
        public decimal Factor { get; set; }
    }
}