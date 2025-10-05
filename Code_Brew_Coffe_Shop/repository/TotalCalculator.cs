using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.repository
{
    public class TotalCaculator<Titems, Tresults>
    {
        private readonly Func<IEnumerable<Titems>, Tresults> _aggregate;
        public TotalCaculator(Func<IEnumerable<Titems>, Tresults> aggregate)
        {
            _aggregate = aggregate;
        }
        public Tresults Compute(IEnumerable<Titems> items) => _aggregate(items);
    }
}
