﻿using Hospital.Models.Manager;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Manager
{
    public class EquipmentOrderRepository
    {
        private const string FilePath = "../../../Data/equipmentOrders.csv";
        private const string OrderItemFilePath = "../../../Data/equipmentOrderItems.csv";

        private static EquipmentOrderRepository? _instance;
        private List<EquipmentOrder>? _orders = null;

        private EquipmentOrderRepository()
        {
            
        }

        public static EquipmentOrderRepository Instance
        {
            get { return _instance ??= new EquipmentOrderRepository(); }
        }

        public void AddItemsFromCsv(List<EquipmentOrder> orders)
        {
            List<EquipmentOrderItem> allEquipmentOrderItems = Serializer<EquipmentOrderItem>.FromCSV(OrderItemFilePath);

            foreach (var orderItem in allEquipmentOrderItems)
            {
                EquipmentOrder? order = orders.Find(order => order.Id == orderItem.OrderId);
                if (order != null)
                {
                    order.Items.Add(orderItem);
                }
            }
        }

        public List<EquipmentOrder> GetAll()
        {
            if (_orders == null)
            {
                _orders = Serializer<EquipmentOrder>.FromCSV(FilePath);
                AddItemsFromCsv(_orders);

            }
            return _orders;
        }

        public void Add(EquipmentOrder order)
        {
            var orders = GetAll();
            orders.Add(order);
            WriteOrderItemsFromOrdersToCSV(orders);
            Serializer<EquipmentOrder>.ToCSV(orders, FilePath);
        }

        private void WriteOrderItemsFromOrdersToCSV(List<EquipmentOrder> orders)
        {
            var orderItems = new List<EquipmentOrderItem>();


            foreach (var orderInList in orders)
            {
                orderItems.AddRange(orderInList.Items);
            }

            Serializer<EquipmentOrderItem>.ToCSV(orderItems, OrderItemFilePath);
        }
        public void Update(EquipmentOrder order)
        {
            var orders = GetAll();
            var orderToUpdate = orders.Find((o) => o.Id == order.Id);
            if (orderToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            orderToUpdate.DeliveryDateTime = order.DeliveryDateTime;
            orderToUpdate.Items = order.Items;
            Serializer<EquipmentOrder>.ToCSV(orders, FilePath);
            WriteOrderItemsFromOrdersToCSV(orders);
        }

        public void DeleteAll()
        {
            GetAll();
            _orders.Clear();
            Serializer<EquipmentOrder>.ToCSV(_orders, FilePath);
            WriteOrderItemsFromOrdersToCSV(_orders);
            _orders = null;

        }
        
    }
}
