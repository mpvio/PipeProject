using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace PipeProject.Models
{
    public class Pipe: BindableBase
    {
        [Required(ErrorMessage = "Label is required")]
        [MinLength(1, ErrorMessage = "Label cannot be empty")]
        [StringLength(16, ErrorMessage = "Label must be 16 characters or less")]
        public string Label { get; set; }

        private PipeTypeEnum _pipeType = PipeTypeEnum.AnsiB3610_20;
        private double _youngsModulus = 200.0;

        [Required(ErrorMessage = "Pipe type is required")]
        public PipeTypeEnum PipeType
        {
            get => _pipeType;
            set
            {
                if (SetProperty(ref _pipeType, value)) { }
            }
        }

        [Range(0.0001, double.MaxValue, ErrorMessage = "Young's Modulus must be greater than 0")]
        public double YoungsModulus
        {
            get => _youngsModulus;
            set => SetProperty(ref _youngsModulus, value);
        }

        [Range(0.0001, double.MaxValue, ErrorMessage = "Diameter must be greater than 0")]
        public double Diameter { get; set; } = 10.0; // cm default

        [Range(0.0001, double.MaxValue, ErrorMessage = "Length must be greater than 0")]
        public double Length { get; set; } = 100.0; // m default

        [Range(0.0001, 1500.0, ErrorMessage = "Wave speed must be between 0 and 1500 m/sec")]
        public double WaveSpeed { get; set; } = 750.0; // m/sec default
    }
}
