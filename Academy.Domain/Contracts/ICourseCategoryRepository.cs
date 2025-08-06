using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.Category;

namespace Academy.Domain.Contracts;

public interface ICourseCategoryRepository : IEfRepository<CourseCategory, short>;