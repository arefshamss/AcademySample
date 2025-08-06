using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Category;

namespace Academy.Data.Repositories;

public class CourseCategoryRepository(AcademyContext context) : EfRepository<CourseCategory, short>(context), ICourseCategoryRepository;