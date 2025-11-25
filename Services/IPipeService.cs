using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PipeProject.Models;

namespace PipeProject.Services
{
    public interface IPipeService
    {
        List<PipeType> GetPipeTypes();
        void AddPipe(Pipe pipe);
        void UpdatePipe(Pipe pipe);
        void DeletePipe(string label);
        List<Pipe> GetAllPipes();
        Pipe GetPipeByLabel(string label);
        bool LabelExists(string label);

        // save/ load to json
        void SavePipesToFile(string filePath);
        void LoadPipesFromFile(string filePath);
    }
}
