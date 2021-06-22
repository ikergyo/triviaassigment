using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ITriviaApi
{
    public Task<Question[]> GetQuestions(string amount, string category);
    public Task<List<Category>> GetCategories();
}
