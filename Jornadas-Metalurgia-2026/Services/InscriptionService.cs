using AutoMapper;
using Jornadas_Metalurgia_2026.Models.Inscription;
using Jornadas_Metalurgia_2026.Models.Inscription.DTO;
using Jornadas_Metalurgia_2026.Repositories;
using Jornadas_Metalurgia_2026.Utils;
using Jornadas_Metalurgia_2026.Enum;
using System.Net;

namespace Jornadas_Metalurgia_2026.Services
{
    public class InscriptionService
    {


        private readonly IInscriptionRepository _repo;
        private readonly IMapper _mapper;

        public InscriptionService(IInscriptionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        //En este servicio se pueden crear, modidicar, poner inactivas y traer todas las inscripciones


        public async Task<Inscription> CreateOneInscription(InscriptionCreateDTO dto)
        {
            Inscription newInscription;

            //En primer lugar le preguntas el tipo
            if (dto.InscriptionType == INSCRIPTION.PRESENTATION)
            {

                newInscription = new PresentationInscription
                {
                    StudentName = dto.StudentName,
                    StudentEmail = dto.StudentEmail,
                    StudentDNI = dto.StudentDni,
                    StudentInstitution = dto.StudentInstitution,

                    PresentationTitle = dto.PresentationTitle!,
                    PresentationParticipants = dto.Participants!,
                    Presentation = dto.Presentation!
                };
            }
            else
            {
                newInscription = new AttendanceInscription
                {
                    StudentName = dto.StudentName,
                    StudentEmail = dto.StudentEmail,
                    StudentDNI = dto.StudentDni,
                    StudentInstitution = dto.StudentInstitution

                };
            }
            await _repo.CreateOneAsync(newInscription);
            return newInscription;
        }



        public async Task DeleteOneById(string id)
        {
            if (!int.TryParse(id, out int inscriptionId))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "ID de inscripcin inválido");

            }

            var inscription = await _repo.GetOneAsync(I => I.Id == inscriptionId);
            if (inscription == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, "Inscripcón no encontrada");

            }
            else
            {
                inscription.IsActive = false;

            }
            await _repo.DeleteOneAsync(inscription);
        }



        public async Task<Inscription> UpdateInscription(string id, InscriptionUpdateDTO dto)
        {
            if (!int.TryParse(id, out int inscriptionId))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "ID de inscripcin inválido");

            }

            var inscription = await _repo.GetOneAsync(I => I.Id == inscriptionId);
            if (inscription == null)
            {

                throw new HttpResponseError(HttpStatusCode.NotFound, "Inscripcón no encontrada");
            }

            if (dto.InscriptionType == INSCRIPTION.PRESENTATION)
            {
                if (inscription is PresentationInscription presentation)
                {
                    presentation.PresentationParticipants = dto.Participants!;
                    presentation.PresentationTitle = dto.PresentationTitle;
                    presentation.Presentation = dto.Presentation;
                }
                else
                {
                    throw new HttpResponseError(HttpStatusCode.BadRequest, "El tipo de inscripción no coincide");
                }
            }
            else
            {
                inscription.StudentEmail = dto.StudentEmail;
            }

            await _repo.UpdateOneAsync(inscription);
            return inscription;
        }

        //metodo que trae todas las inscripciones segun filtros 
        public async Task<List<Inscription>> GetAll(string? tipo, bool? isactive = true)

        {
            var inscriptions = await _repo.GetAllAsync();
            if (isactive.HasValue)
            {
                inscriptions = inscriptions.Where(i => i.IsActive == isactive.Value);
            }
            if (!string.IsNullOrEmpty(tipo))
            {

                switch (tipo)
                {
                    case INSCRIPTION.PRESENTATION:

                        inscriptions = inscriptions.Where(i => i is PresentationInscription).ToList();
                        break;

                    case INSCRIPTION.ATTENDANCE:

                        inscriptions = inscriptions.Where(i => i is AttendanceInscription).ToList();

                        break;
                }


            }
            return inscriptions.ToList();

        }
    }
}
