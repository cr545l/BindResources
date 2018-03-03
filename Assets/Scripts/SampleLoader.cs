using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleLoader : MonoBehaviour
{
	private void Start()
	{
		Lofle.BindResources.Instantaite<Cube>();
	}
}
