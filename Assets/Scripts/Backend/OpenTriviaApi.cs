using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class OpenTriviaApi : ITriviaApi
{
    // Example query https://opentdb.com/api.php?amount=10&category=9&difficulty=hard

    const string categoryQuery = "https://opentdb.com/api_category.php";
    const string baseUri = "https://opentdb.com/api.php";

    UriBuilder uri;
    HttpClient client;
    Dictionary<string, string> content;

    List<Category> categories;

    public OpenTriviaApi()
    {
        uri = new UriBuilder(baseUri);
        content = new Dictionary<string, string>();
        client = new HttpClient();
    }
    /// <summary>
    /// Creating a request to get the questions from OpenTrivia DB
    /// </summary>
    /// <param name="amount">Amount of the questions</param>
    /// <param name="category">Category</param>
    /// <returns>Returns the collected questions</returns>
    public async Task<Question[]> GetQuestions(string amount, string category = null)
    {
        content.Add("amount", amount.ToString());
        if (!string.IsNullOrEmpty(category))
            AddCategory(category);

        GenerateUri();

        string response = await client.GetStringAsync(uri.ToString());
        QuestionList ql = JsonUtility.FromJson<QuestionList>(response);
        return ql.results;
    }
    /// <summary>
    /// Creating a reuqest to get the categories from OpenTrivia DB
    /// </summary>
    /// <returns>Returns the categories</returns>
    public async Task<List<Category>> GetCategories()
    {
        string response = await client.GetStringAsync(categoryQuery);

        CategoryList ctr = JsonUtility.FromJson<CategoryList>(response);

        categories = ctr.trivia_categories;
        return categories;
    }
    /// <summary>
    /// Add category to the content if it is in the list.
    /// </summary>
    /// <param name="category">Name of the category</param>
    void AddCategory(string category)
    {
        for (int i = 0; i < categories.Count; i++)
        {
            if (categories[i].name == category)
            {
                content.Add("category", categories[i].id.ToString());
                return;
            }
        }
    }
    /// <summary>
    /// Generateing the final uri from the content dictionary
    /// </summary>
    void GenerateUri()
    {
        foreach (var item in content)
        {
            AddParameter($"{item.Key}={item.Value}");
        }
        content.Clear();
    }
    /// <summary>
    /// Adding parameter to the uri string
    /// </summary>
    /// <param name="param">Parameter</param>
    void AddParameter(string param)
    {
        if (uri.Query != null && uri.Query.Length > 1)
            uri.Query = uri.Query.Substring(1) + "&" + param;
        else
            uri.Query = param;
    }

}
