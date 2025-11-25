using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PipeProject.Models;

namespace PipeProject.Services
{
    public class PipeService : IPipeService
    {
        private readonly List<Pipe> _pipes = new List<Pipe>();
        public List<PipeType> GetPipeTypes()
        {
            return Enum.GetValues(typeof(PipeTypeEnum))
                      .Cast<PipeTypeEnum>()
                      .Select(pt => new PipeType { Type = pt })
                      .ToList();
        }

        public PipeType GetPipeType(PipeTypeEnum pipeTypeEnum)
        {
            return new PipeType { Type = pipeTypeEnum };
        }

        public void AddPipe(Pipe pipe)
        {
            if (LabelExists(pipe.Label))
                throw new ArgumentException($"Pipe with label '{pipe.Label}' already exists");

            _pipes.Add(pipe);
        }

        public void UpdatePipe(Pipe pipe)
        {
            var existingPipe = _pipes.FirstOrDefault(p => p.Label == pipe.Label);
            if (existingPipe != null)
            {
                _pipes.Remove(existingPipe);
                _pipes.Add(pipe);
            }
        }

        public void DeletePipe(string label)
        {
            var pipe = _pipes.FirstOrDefault(p => p.Label == label);
            if (pipe != null)
                _pipes.Remove(pipe);
        }

        public List<Pipe> GetAllPipes() => _pipes.ToList();

        public Pipe GetPipeByLabel(string label) =>
            _pipes.FirstOrDefault(p => p.Label == label);

        public bool LabelExists(string label) =>
            _pipes.Any(p => p.Label == label);

        public void SavePipesToFile(string filePath)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_pipes, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving pipes to file: {ex.Message}", ex);
            }
        }

        public void LoadPipesFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Pipe data file not found");

                var json = File.ReadAllText(filePath);
                var loadedPipes = JsonConvert.DeserializeObject<List<Pipe>>(json);

                if (loadedPipes != null)
                {
                    _pipes.Clear();  
                }

                _pipes.AddRange(loadedPipes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading pipes from file: {ex.Message}", ex);
            }
        }
    }
}
