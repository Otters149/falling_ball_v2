using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace data
{
    namespace entity
    {
        [Serializable]
        public class Shop_Item
        {
            public string id;
            public string place_holder;
            public int price;
            public bool is_bought;
        }
    }
}
