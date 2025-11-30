using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadManager
{
    void Save();
    void Load();
    void Delete();
    bool IsFileExist();
}
