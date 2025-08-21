using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharHandler
{
    public void ParseAndStore(LitJson.JsonData data);
}
