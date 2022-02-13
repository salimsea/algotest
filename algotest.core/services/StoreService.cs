using System;
using System.Collections.Generic;
using System.Linq;
using algotest.core.constants;
using algotest.core.entities;
using algotest.core.models;
using Dapper;

namespace algotest.core.services
{
    public interface IStoreService
    {
        UserModel Login(string email, string password, out string oMessage);
        string Register(UserAddModel data, out string oMessage);
        UserModel GetUser(int userId, out string oMessage);


        #region MANAGE PRODUCT
        string AddProduct(ProductAddModel data, out string oMessage);
        string UpdateProduct(ProductUpdateModel data, out string oMessage);
        string DeleteProduct(int productId, out string oMessage);
        ProductModel GetProduct(int productId, out string oMessage);
        List<ProductModel> GetProducts(out string oMessage);
        #endregion

        string AddOrder(int userId, OrderAddModel data, out string oMessage);
    }
    public class StoreService : IStoreService
    {
        private readonly string ServiceName = "StoreService.";
        private readonly ICommonService commonService;
        public StoreService(ICommonService commonService)
        {
            this.commonService = commonService;
        }


        public UserModel Login(string email, string password, out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                using var conn = commonService.DbConnection();
                var users = conn.GetList<TbUsers>().Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault();
                if (users == null)
                {
                    oMessage = "email not found";
                    return null;
                }
                if (users.Password != password)
                {
                    oMessage = "wrong password";
                    return null;
                }

                return new UserModel
                {
                    Id = users.Id,
                    Name = users.Name,
                    Email = users.Email,
                    Password = users.Password
                };
            }
            catch (Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "Login", ex);
                return null;
            }
        }

        public string Register(UserAddModel data, out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                using var conn = commonService.DbConnection();
                var users = conn.GetList<TbUsers>().Where(x => x.Email.ToLower() == data.Email.ToLower()).FirstOrDefault();
                if (users != null)
                {
                    oMessage = "email already exists";
                    return null;
                }
                users = new TbUsers
                {
                    Name = data.Name,
                    Email = data.Email,
                    Password = data.Password,
                    CreatedAt = DateTime.Now
                };
                conn.Insert(users);

                return null;
            }
            catch (Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "Register", ex);
                return null;
            }
        }

        public UserModel GetUser(int userId, out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                using var conn = commonService.DbConnection();
                var users = conn.Get<TbUsers>(userId);
                if (users == null)
                {
                    oMessage = "user not found";
                    return null;
                }
                return new UserModel
                {
                    Id = users.Id,
                    Name = users.Name,
                    Email = users.Email,
                    Password = users.Password,
                    CreatedAt = users.CreatedAt.ToString("dd-MM-yyyy")
                };
            }
            catch (Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "GetUser", ex);
                return null;
            }
        }

        public string AddOrder(int userId, OrderAddModel data, out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                using (var conn = commonService.DbConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        TbOrders tbOrders = new TbOrders
                        {
                            UserId = userId,
                            Status = StatusOrdersConstant.Delivered,
                            CreatedAt = DateTime.Now
                        };
                        var _idOrders = conn.Insert(tbOrders);
                        if (data.Products.Count == 0)
                        {
                            oMessage = "choose a product";
                            return null;
                        }
                        foreach (var item in data.Products)
                        {
                            TbOrderItems tbOrderItems = new TbOrderItems
                            {
                                OrderId = (int)_idOrders,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity
                            };
                            conn.Insert(tbOrderItems);
                        }
                        tx.Commit();
                        return null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "AddOrder", ex);
                return null;
            }
        }

        public string AddProduct(ProductAddModel data, out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(data.Name))
                {
                    oMessage = "product name required";
                    return null;
                }
                if (data.Price < 0)
                {
                    oMessage = "product price cannot be below zero";
                    return null;
                }
                using (var conn = commonService.DbConnection())
                {
                    TbProducts tbProducts = new TbProducts
                    {
                        Name = data.Name,
                        Price = data.Price,
                        CreatedAt = DateTime.Now
                    };
                    conn.Insert(tbProducts);
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "AddProduct", ex);
                return null;
            }
        }

        public string DeleteProduct(int productId, out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                using (var conn = commonService.DbConnection())
                {
                    var product = conn.Get<TbProducts>(productId);
                    if (product == null)
                    {
                        oMessage = "product does not exists";
                        return null;
                    }
                    conn.Delete(product);
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "DeleteProduct", ex);
                return null;
            }
        }

        public ProductModel GetProduct(int productId, out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                using (var conn = commonService.DbConnection())
                {
                    var product = conn.Get<TbProducts>(productId);
                    if (product == null)
                    {
                        oMessage = "product does not exists";
                        return null;
                    }
                    return new ProductModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        CreatedAt = product.CreatedAt.ToString("dd-MM-yyyy")
                    };
                }
            }
            catch (System.Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "GetProduct", ex);
                return null;
            }
        }

        public List<ProductModel> GetProducts(out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                using (var conn = commonService.DbConnection())
                {
                    var products = conn.GetList<TbProducts>().ToList();
                    if (products.Count == 0)
                    {
                        oMessage = "product data not found";
                        return null;
                    }
                    List<ProductModel> ret = new();
                    foreach (var item in products.OrderByDescending(x => x.Id))
                    {
                        ret.Add(new ProductModel
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Price = item.Price,
                            CreatedAt = item.CreatedAt.ToString("dd-MM-yyyy")
                        });
                    }
                    return ret;
                }
            }
            catch (System.Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "GetProducts", ex);
                return null;
            }
        }

        public string UpdateProduct(ProductUpdateModel data, out string oMessage)
        {
            oMessage = string.Empty;
            try
            {
                using (var conn = commonService.DbConnection())
                {
                    var product = conn.Get<TbProducts>(data.Id);
                    if (product == null)
                    {
                        oMessage = "product does not exists";
                        return null;
                    }
                    product.Name = data.Name;
                    product.Price = data.Price;
                    conn.Update(product);
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                oMessage = commonService.GetErrorMessage(ServiceName + "UpdateProduct", ex);
                return null;
            }
        }
    }
}

