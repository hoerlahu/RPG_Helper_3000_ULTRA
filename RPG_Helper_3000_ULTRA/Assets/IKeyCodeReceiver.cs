using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyCodeReceiver{

    bool HandlesKeyDown(KeyCode key);
    
}
