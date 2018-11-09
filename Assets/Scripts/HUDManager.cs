using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	EnergyManager energyManager;
	public Text Energy;

	void Awake() {
		energyManager = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<EnergyManager>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateEnergy ();
	}

	void UpdateEnergy() {
		// Just drop the decimal on these for now, doesn't matter.
		int energy = (int)energyManager.energy;
		int maxEnergy = (int)energyManager.maxEnergy;
		int lowEnergyWarningThreshold = (int)energyManager.lowEnergyWarningThreshold;
		int lowEnergyDangerThreshold = (int)energyManager.lowEnergyDangerThreshold;

		Energy.text = energy.ToString();

		if (energy >= maxEnergy) {
			Energy.color = Color.green;
		} else if (energy < lowEnergyDangerThreshold) {
			Energy.color = Color.red;
		} else if (energy < lowEnergyWarningThreshold) {
			Energy.color = Color.yellow;
		} else {
			Energy.color = Color.white;
		}
	}
}
