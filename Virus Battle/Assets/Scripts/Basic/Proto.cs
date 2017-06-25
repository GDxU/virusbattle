using UnityEngine;
using System.Collections;

public abstract class Proto<K, V>
    where K : Model
    where V : View
{
    protected K model = global::Model.None as K;
    protected V view = global::View.None as V;

    public Proto(K model)
    {
        this.model = model;
    }
}
