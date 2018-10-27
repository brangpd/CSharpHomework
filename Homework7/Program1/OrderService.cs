﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Program1
{
	public class OrderService
	{
		#region Constructor and instance

		private OrderService()
		{
			ReadStatus();
		}

		~OrderService()
		{
			SaveStatus();
		}

		private static OrderService _instance;

		public static OrderService GetInstance() => _instance ?? (_instance = new OrderService());

		#endregion

		public static string SavingPath { set; get; } = "./OrderService.xml";

		private List<Order> _list { set; get; } = new List<Order>();

        public List<Order> List { get => new List<Order>(_list); }

		public void AddOrder(Order order) => _list.Add(order);

		public bool RemoveOrder(int index)
		{
			if (index < 0 || index >= _list.Count) return false;
            _list.RemoveAt(index);
			return true;
		}

		public bool RemoveAll(Predicate<Order> match)
		{
			bool res = false;
			for (var i = 0; i < _list.Count; ++i)
			{
				if (!match(_list[i])) continue;
				res = true;
                _list.RemoveAt(i--);
			}

			return res;
		}

		public List<Order> FindAll(Predicate<Order> match)
			=> (from order in _list where match(order) select order).ToList();

		public bool ModifyOrder(int index, Order order)
		{
			if (index < 0 || index >= _list.Count) return false;
            _list[index] = order;
			return true;
		}

		public void SaveStatus(string path = "./list.xml")
		{
			var xmlSerializer = new XmlSerializer(_list.GetType());
			using (var fileStream = new FileStream(path, FileMode.Create))
				xmlSerializer.Serialize(fileStream, _list);
		}

		public void ReadStatus(string path = "./list.xml")
		{
			var xmlSerializer = new XmlSerializer(_list.GetType());
			using (var fileStream = new FileStream(path, FileMode.Open))
                _list = (List<Order>) xmlSerializer.Deserialize(fileStream);
		}

		public void ClearStatus(string path = "./list.xml")
		{
			if (File.Exists(path)) File.Delete(path);
            _instance._list.Clear();
		}
	}
}
