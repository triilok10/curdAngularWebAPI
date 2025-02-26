using curdAngularWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace curdAngularWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        // URL: https://localhost:7063/swagger/index.html

        #region "Connection String"
        public readonly string _ConnectionString;
        public ProductAPIController(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("CustomConnection");
        }
        #endregion

        #region "Insert Record"
        [HttpPost("InsertRecord")]
        public IActionResult InsertRecord([FromBody] ProductItem pProductItem)
        {
            ProductResponse response = new ProductResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("usp_ProductItem", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 1); // 1 for Insert
                        cmd.Parameters.AddWithValue("@ProductName", pProductItem.ProductName);
                        cmd.Parameters.AddWithValue("@Price", pProductItem.Price);
                        cmd.Parameters.AddWithValue("@Rating", pProductItem.Rating);
                        cmd.Parameters.AddWithValue("@Description", pProductItem.Description);
                        cmd.Parameters.AddWithValue("@Status", pProductItem.Status);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                response.Message = Convert.ToString(rdr["Message"]);
                                response.APIStatus = Convert.ToInt32(rdr["APIStatus"]);
                                response.ProductItems = null;
                            }
                        }
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                response.Message = ex.Message;
                response.APIStatus = 2;
                response.ProductItems = null;
                return StatusCode(500, response);
            }
        }
        #endregion

        #region "Get Product List"
        [HttpGet("GetList")]
        public IActionResult GetList()
        {
            ProductResponse obj = new ProductResponse();
            obj.ProductItems = new List<ProductItem>();

            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("usp_ProductItem", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 2); // 2 for Get List

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ProductItem productItem = new ProductItem
                                {
                                    ProductItemID = Convert.ToInt32(rdr["ProductItemID"]),
                                    ProductName = rdr["ProductName"].ToString(),
                                    Price = Convert.ToInt32(rdr["Price"]),
                                    Rating = Convert.ToInt32(rdr["Rating"]),
                                    Description = rdr["Description"].ToString(),
                                    Status = Convert.ToBoolean(rdr["Status"])
                                };

                                obj.ProductItems.Add(productItem);
                            }
                        }
                    }
                }

                obj.APIStatus = 1;
                obj.Message = "Products retrieved successfully.";
                return Ok(obj);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                obj.APIStatus = 2;
                obj.Message = "Error retrieving products.";
                return StatusCode(500, obj);
            }
        }
        #endregion

        #region "Get Record"
        [HttpGet("GetRecord")]
        public IActionResult GetRecord(int ProductItemID)
        {
            ProductResponse response = new ProductResponse();
            response.ProductItems = new List<ProductItem>();
            ProductItem productItem = new ProductItem();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ProductItem", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 3); // 3 for Get Record
                    cmd.Parameters.AddWithValue("@ProductItemID", ProductItemID);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            productItem.ProductItemID = Convert.ToInt32(rdr["ProductItemID"]);
                            productItem.ProductName = Convert.ToString(rdr["ProductName"]);
                            productItem.Price = Convert.ToInt32(rdr["Price"]);
                            productItem.Rating = Convert.ToInt32(rdr["Rating"]);
                            productItem.Description = Convert.ToString(rdr["Description"]);
                            productItem.Status = Convert.ToBoolean(rdr["Status"]);

                            response.ProductItems.Add(productItem);
                            response.APIStatus = 1;
                            response.Message = productItem.ProductName + " retrieved successfully.";
                            return Ok(response);
                        }
                        else
                        {
                            response.APIStatus = 0;
                            response.Message = "Product not found.";
                            return NotFound(response);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                response.APIStatus = 2;
                response.Message = $"Database error retrieving product,{ex.Message}";
                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                response.APIStatus = 2;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
        }
        #endregion

        #region "Update Record"
        [HttpPut("UpdateRecord/{ProductItemID}")]
        public IActionResult UpdateRecord(int ProductItemID, [FromBody] ProductItem pProductItem)
        {
            ProductResponse response = new ProductResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("usp_ProductItem", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 4); // 4 for Update
                        cmd.Parameters.AddWithValue("@ProductItemID", ProductItemID);
                        cmd.Parameters.AddWithValue("@ProductName", pProductItem.ProductName);
                        cmd.Parameters.AddWithValue("@Price", pProductItem.Price);
                        cmd.Parameters.AddWithValue("@Rating", pProductItem.Rating);
                        cmd.Parameters.AddWithValue("@Description", pProductItem.Description);
                        cmd.Parameters.AddWithValue("@Status", pProductItem.Status);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                response.Message = Convert.ToString(rdr["Message"]);
                                response.APIStatus = Convert.ToInt32(rdr["APIStatus"]);
                                response.ProductItems = null;
                            }
                        }
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                response.Message = ex.Message;
                response.APIStatus = 2;
                response.ProductItems = null;
                return StatusCode(500, response);
            }
        }
        #endregion

        #region "Delete Record"
        [HttpDelete("DeleteRecord/{ProductItemID}")]
        public IActionResult DeleteRecord(int ProductItemID)
        {
            ProductResponse response = new ProductResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("usp_ProductItem", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 5); // 5 for Delete
                        cmd.Parameters.AddWithValue("@ProductItemID", ProductItemID);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                response.Message = Convert.ToString(rdr["Message"]);
                                response.APIStatus = Convert.ToInt32(rdr["APIStatus"]);
                                response.ProductItems = null;
                            }
                        }
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                response.Message = ex.Message;
                response.APIStatus = 2;
                response.ProductItems = null;
                return StatusCode(500, response);
            }
        }
        #endregion


        #region "Insert Record PostAPI"

        [HttpPost("PostAPI")]
        public async Task<PostAPIResponse> PostAPI(PostAPI pPostAPI)
        {
            PostAPIResponse obj = new PostAPIResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    await con.OpenAsync();

                    SqlCommand cmd = new SqlCommand("usp_InsertPostAPI", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };


                    cmd.Parameters.AddWithValue("@Mode", "AddPostAPI");
                    cmd.Parameters.AddWithValue("@firstName", pPostAPI.firstName);
                    cmd.Parameters.AddWithValue("@lastName", pPostAPI.lastName);
                    cmd.Parameters.AddWithValue("@address", pPostAPI.address);
                    cmd.Parameters.AddWithValue("@mobileNumber", pPostAPI.mobileNumber);
                    cmd.Parameters.AddWithValue("@gender", pPostAPI.gender);


                    using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            obj.postApiID = Convert.ToInt32(rdr["postApiID"]);
                            obj.status = Convert.ToBoolean(rdr["status"]);
                            obj.message = Convert.ToString(rdr["message"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                obj.status = false;
                obj.message = ex.Message;
                obj.postApiID = 0;
            }
            return obj;
        }

        #endregion


        #region "Insert Record PostAPI"

        [HttpGet("GetAPI")]
        public async Task<MasterResponse> GetAPI()
        {
            MasterResponse obj = new MasterResponse();
            List<GetApiResponse> lstPost = new List<GetApiResponse>();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    await con.OpenAsync();


                    SqlCommand cmd = new SqlCommand("usp_InsertPostAPI", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@Mode", "GETLIST");

                    using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            lstPost.Add(new GetApiResponse
                            {
                                postApiID = Convert.ToInt32(rdr["postApiID"]),
                                firstName = Convert.ToString(rdr["firstName"]),
                                lastName = Convert.ToString(rdr["lastName"]),
                                mobileNumber = Convert.ToInt32(rdr["mobileNumber"]),
                                address = Convert.ToString(rdr["address"]),
                                gender = Convert.ToString(rdr["gender"])
                            });
                        }
                    }


                    obj.status = true;
                    obj.message = "Success";
                    obj.totalRecord = lstPost.Count;
                    if (lstPost.Count > 0)
                    {
                        obj.ProductItems = lstPost;
                    }
                    else
                    {
                        obj.ProductItems = null;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                obj.message = ex.Message;
            }
            return obj;
        }


        #endregion
    }
}
