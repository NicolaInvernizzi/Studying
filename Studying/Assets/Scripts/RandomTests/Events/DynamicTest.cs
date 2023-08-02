using UnityEngine.Events;
using System;

[Serializable]
public class DynamicTest : UnityEvent<int, bool> { }

[Serializable]
public class DynamicTest2 : UnityEvent<bool> { }

