using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrate
{
    public class SliderManager : ISliderService
    {
        ISlider _slider;

        public SliderManager(ISlider slider)
        {
            _slider = slider;
        }

        public void TAdd(Slider t)
        {
            _slider.Create(t);
        }

        public void TDelete(Slider t)
        {
            _slider.Delete(t);
        }

        public Slider TGetByID(int id)
        {
            return _slider.GetById(id);
        }

        public List<Slider> TGetList()
        {
            return _slider.GetList();
        }

        public void TUpdate(Slider t)
        {
           _slider.Update(t);
        }
    }
}
