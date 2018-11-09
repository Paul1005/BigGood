using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ControlBinding : IList<KeyCode> {
	private IList<KeyCode> _list = new List<KeyCode>();

	public string AllKeysToString()
	{
		StringBuilder sb = new StringBuilder ();
		for(int i = 0; i < _list.Count;i++)
		{
			sb.Append (_list[i]);
			if (i != _list.Count - 1)
			{
				sb.Append (" ");
			}
		}
		return sb.ToString ();
	}

	// Implementation of IEnumerable
	public IEnumerator<KeyCode> GetEnumerator()
	{
		return _list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	// Implementation of ICollection
	public void Add(KeyCode item)
	{
		_list.Add(item);
	}

	public void Clear()
	{
		_list.Clear();
	}

	public bool Contains(KeyCode item)
	{
		return _list.Contains(item);
	}

	public void CopyTo(KeyCode[] array, int arrayIndex)
	{
		_list.CopyTo(array, arrayIndex);
	}

	public bool Remove(KeyCode item)
	{
		return _list.Remove(item);
	}

	public int Count
	{
		get { return _list.Count; }
	}

	public bool IsReadOnly
	{
		get { return _list.IsReadOnly; }
	}

	// Implementation of IList
	public int IndexOf(KeyCode item)
	{
		return _list.IndexOf(item);
	}

	public void Insert(int index, KeyCode item)
	{
		_list.Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		_list.RemoveAt(index);
	}

	public KeyCode this[int index]
	{
		get { return _list[index]; }
		set { _list[index] = value; }
	}
}
