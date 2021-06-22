using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CategoryList
{
    public List<Category> trivia_categories;
}

[Serializable]
public class Category
{
    public string id;
    public string name;

    public override string ToString()
    {
        return "ID: " + id + ", Name: " + name;
    }
}
